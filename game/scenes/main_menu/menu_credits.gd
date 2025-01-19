extends CanvasLayer
class_name Credits

@onready var back_button: Button = $Control/Panel/MarginContainer/VBoxContainer/BACK_Button

func _ready() -> void:
	# Starts Menu Music.
	AudioManager.play_music("main_menu")

	# Manually connecting hover sounds.
	back_button.mouse_entered.connect(call_highlight_sound)

	# Manually connecting clicked sounds.
	back_button.pressed.connect(call_clicked_sound)
	
func _back_button_pressed() -> void:
	SceneManager.to_main_menu()

func call_highlight_sound():
	AudioManager.play_UI_sound("highlight")

func call_clicked_sound():
	AudioManager.play_UI_sound("clicked")
