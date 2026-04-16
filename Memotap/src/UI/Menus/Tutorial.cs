
using Godot;
using System;

public partial class Tutorial : Control
{

	[Export] Game _game;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Show();
	}

	public void OnStartPressed()
	{
		Hide();
		_game.StartGame();

	}


	// Called when QuitButton is pressed
	public void OnQuitPressed()
	{
		Hide();
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}
}
