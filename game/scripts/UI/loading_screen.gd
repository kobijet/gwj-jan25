extends CanvasLayer
class_name LoadingScreen
signal fade_out_finished
signal fade_in_finished

# Simple loading screen with a color rect, and an animation player.
# Animation player has two animations; "fade_in", and "fade_out".

# Animation Player Reference
@onready var animation_player: AnimationPlayer = %AnimationPlayer

# Bools For Checking Fade Type
var fading_out : bool = false
var fading_in : bool = false

# Plays the fade out animation.
func fade_out() -> void:
	fading_out = true
	animation_player.play("fade_out")

# Plays the fade in animation.
func fade_in() -> void:
	fading_in = true
	animation_player.play("fade_in")

# Emits correct signals on an animation finished depending on the original function called; fade_out, or fade_in.
func _report_fade_state(_blank) -> void:
	if fading_out:
		fade_out_finished.emit()
		fading_out = false
	if fading_in:
		fade_in_finished.emit()
		fading_in = false
