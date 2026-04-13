extends Node2D

var languageNames = ["English","Suomi"]
var languageCodes = ["en","fi"]

func _ready():
	updateUI()

func _on_button_english_pressed():
	TranslationServer.set_locale("en")
	updateUI()

func _on_button_suomi_button_up():
	TranslationServer.set_locale("fi")
	updateUI()

	func updateUI():

	$Label.text = tr("Level")
	$Label.text = tr("Game over")
	$Label.text = tr("Lorem Lipsum Tutorial in english Lorem Lipsum")
	$Label.text = tr("START OVER")
	$Label.text = tr("EXIT")
	$Label.text = tr("SOUND OFF")
	$Label.text = tr("SOUND ON")
	$Label.text = tr("CONTINUE")
	$ButtonEnglish.text = tr("EGNLISH")
	$ButtonSuomi.text = tr("SUOMI")















# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
