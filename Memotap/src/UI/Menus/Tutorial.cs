
using Godot;
using System;

public partial class Tutorial : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Tutorial script loaded");


	}

	// Called when PauseButton is pressed
	public void OnPausePressed()
	{
		Engine.TimeScale = 0;

		GetTree().Paused = true;
		Show();
	}

	// Sets 'paused' to false and hides the pause-menu
	public void OnStartPressed()
	{
		Engine.TimeScale = 1;

		GetTree().Paused = false;
		Hide();
	}


	// Called when QuitButton is pressed
	public void OnQuitPressed()
	{
		GD.Print("guit has been pressed");
		Hide();
		//GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}
}
