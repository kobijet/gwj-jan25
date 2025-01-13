extends CanvasLayer
class_name MainMenu

@onready var start_button: Button = $Control/Panel/MarginContainer/VBoxContainer/START_Button
@onready var exit_button: Button = $Control/Panel/MarginContainer/VBoxContainer/EXIT_Button

func _ready() -> void:
	# Starts Menu Music.
	AudioManager.play_music("main_menu")

	# Manually connecting hover sounds.
	start_button.mouse_entered.connect(call_highlight_sound)
	exit_button.mouse_entered.connect(call_highlight_sound)

	# Manually connecting clicked sounds.
	start_button.pressed.connect(call_clicked_sound)
	exit_button.pressed.connect(call_clicked_sound)

func _start_button_pressed() -> void:
	SceneManager.to_gameplay()

func _exit_button_pressed() -> void:
	get_tree().quit()

func call_highlight_sound():
	AudioManager.play_UI_sound("highlight")

func call_clicked_sound():
	AudioManager.play_UI_sound("clicked")
