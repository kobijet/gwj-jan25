extends Node

# Scene change duration.
@export var scene_change_duration: float = 3

# Scenes to be loaded. Add more as necessary with the proper function.
@export var main_menu : PackedScene
@export var gameplay : PackedScene

# Timer used to determine the time given for loading out/in.
@onready var scene_change_timer: Timer = $SceneChangeTimer

# Simple Canvas Node To Cover Everything. Has functions, 'fade_in', and 'fade_out'.
var load_screen_instance : LoadingScreen
const LOADING_SCREEN = preload("res://game/scenes/UI/loading_screen.tscn")

# Current Scene, and Next Scene to go to.
var current_scene : Node
var next_scene : PackedScene

# Canvas Node, to instantiate during the pause menu.
var pause_menu_instance : PauseScreen
const PAUSE_SCREEN = preload("res://game/scenes/UI/pause_screen.tscn")

# Dictionary of scenes. Add more as needed.
var known_scenes: Dictionary = {
	"main_menu": preload("res://game/scenes/main_menu/main_menu.tscn"),
	"gameplay": preload("res://game/scenes/gameplay.tscn"),
	"credits": preload("res://game/scenes/main_menu/menu_credits.tscn")
}

# Main Menu Check Boolean; Duct Tape Fix.
var on_main_menu : bool = true
# Paused Check; Another Duct Tape Fix. :D
var paused : bool = false

func _input(event: InputEvent) -> void:
	# Pressing the input mapped key checks if the scene tree is already paused, if not it runs the pause function.
	if event.is_action_pressed("pause"):
		if !get_tree().paused:
			pause_game()


# Goes to main menu.
func to_main_menu() -> void:
	# Starts Menu Music.
	AudioManager.play_music("main_menu")

	on_main_menu = true
	next_scene = known_scenes["main_menu"]
	scene_change()
	
# Goes to credits menu.
func to_credits() -> void:
	on_main_menu = true
	next_scene = known_scenes["credits"]
	scene_change()

# Goes to gameplay.
func to_gameplay() -> void:
	# Starts Gameplay Music.
	AudioManager.play_music("gameplay")

	on_main_menu = false
	next_scene = known_scenes["gameplay"]
	scene_change()

# Starts timer, and called
func scene_change() -> void:
	if scene_change_timer.is_stopped():
		load_screen_instance = LOADING_SCREEN.instantiate()
		add_child(load_screen_instance)
		Input.mouse_mode = Input.MOUSE_MODE_HIDDEN
		load_screen_instance.fade_out_finished.connect(load_scene)
		load_screen_instance.fade_in_finished.connect(clear_load_screen)
		load_screen_instance.fade_out()
	else:
		return

# Load scene, and waits until next game loop to call the scene change being finished.
# Might change this if it causes issues, but it should work for a small game.
func load_scene() -> void:
	get_tree().change_scene_to_packed(next_scene)
	await get_tree().physics_frame
	scene_change_finished()

# Uses built in signal for the timer node.
func scene_change_finished() -> void:
	load_screen_instance.fade_in()
	Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

# Clears load screen.
func clear_load_screen() -> void:
	load_screen_instance.queue_free()

# Call pause menu. The rest will be handled by the pause menu node itself.
func pause_game():
	if !on_main_menu:
		pause_menu_instance = PAUSE_SCREEN.instantiate()
		add_child(pause_menu_instance)
