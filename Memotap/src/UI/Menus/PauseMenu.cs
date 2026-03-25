using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Pauses the game and shows the pause-menu
	public void Pause()
	{

		Engine.TimeScale = 0;

		GetTree().Paused = true;
		Visible = true;
	}

	// Sets 'paused' to false and hides the pause-menu
	public void Resume()
	{

		Engine.TimeScale = 1;

		GD.Print("resume called");
		GetTree().Paused = false;
		Visible = false;
	}

	// Reloads the current scene
	public void Restart()
	{
		Resume();
		GetTree().ReloadCurrentScene();
	}

	public void Back()
	{
		//mystistä
		Resume();
		GD.Print("quit FUNC Called");
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}
}
