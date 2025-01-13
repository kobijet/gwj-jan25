extends Node

## Syntax error can happen here; just double check if it's correct.
## If it becomes a problem, we can refactor into static variables at _ready(). :D

# Dictionary of music. Add more as needed.
var music_dict: Dictionary = {
	"main_menu": preload("res://assets/audio/music/Ether Vox.mp3"),
	"gameplay": preload("res://assets/audio/music/Professor Umlaut.mp3"),
	"paused": preload("res://assets/audio/music/Ossuary 5 - Rest.mp3"),
	"shop": preload("res://assets/audio/music/Alien Restaurant.mp3")
}

# Dictionary of UI Sounds. Add more as needed.
var ui_sound_dict: Dictionary = {
	"highlight": preload("res://assets/audio/ui/HighlightSquash.mp3"),
	"clicked": preload("res://assets/audio/ui/ClickedSquish.mp3"),
	"swoosh": preload("res://assets/audio/ui/Swoosh.mp3")
}

@onready var music: AudioStreamPlayer = $Music
@onready var ui: AudioStreamPlayer = $UI

# Fluff variable to keep playing pause music at new time. Reason: It sounded better.
var stored_pause_music_time : float = 0

## Current Song Syntax String List: 'main_menu', 'gameplay', 'paused', 'shop'.
func play_music(song_name:String, play_time:float = 0) -> void:
	music.stream = music_dict[song_name]
	music.play(play_time)
	return

## Current UI Syntax String List: 'highlight', 'clicked', 'swoosh'.
func play_UI_sound(ui_sound_name:String) -> void:
	ui.stream = ui_sound_dict[ui_sound_name]
	ui.play()
