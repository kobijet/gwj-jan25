extends CanvasLayer
class_name PauseScreen

@onready var exit_confirmation: Control = %ExitConfirmation
@onready var animation_player: AnimationPlayer = %AnimationPlayer

@onready var resume_button: Button = %RESUME_Button
@onready var exit_button: Button = %EXIT_Button
@onready var button_yes: Button = %BUTTON_Yes
@onready var button_no: Button = %BUTTON_No

# Used to grab the previous gameplays music time to resume at correct time.
var stored_music_time : float = 0

# Pauses the game when it's instantiated in.
func _ready() -> void:
	# Plays swoosh sound for menu.
	AudioManager.play_UI_sound("swoosh")
	
	# Starts Menu Music.
	stored_music_time = AudioManager.music.get_playback_position()
	AudioManager.play_music("paused", AudioManager.stored_pause_music_time)

	get_tree().paused = true
	animation_player.play("slide_on_to_screen")
	
	# Manually connecting hover sounds.
	resume_button.mouse_entered.connect(call_highlight_sound)
	exit_button.mouse_entered.connect(call_highlight_sound)
	button_yes.mouse_entered.connect(call_highlight_sound)
	button_no.mouse_entered.connect(call_highlight_sound)

	# Manually connecting clicked sounds.
	resume_button.pressed.connect(call_clicked_sound)
	exit_button.pressed.connect(call_clicked_sound)
	button_yes.pressed.connect(call_clicked_sound)
	button_no.pressed.connect(call_clicked_sound)

func resume_game() -> void:
	get_tree().paused = false
	AudioManager.stored_pause_music_time = AudioManager.music.get_playback_position()
	AudioManager.play_music("gameplay", stored_music_time)
	self.queue_free()

func exit_game() -> void:
	print('clicked exit game')
	exit_confirmation.visible = true

func confirm_exit() -> void:
	get_tree().paused = false
	SceneManager.to_main_menu()
	queue_free()

func cancel_exit() -> void:
	exit_confirmation.visible = false

func call_highlight_sound():
	AudioManager.play_UI_sound("highlight")

func call_clicked_sound():
	AudioManager.play_UI_sound("clicked")
