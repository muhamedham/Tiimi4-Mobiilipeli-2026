using Godot;
using System;

public partial class MenuController : ColorRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// make sure the pause menu or gameover menu is not visible when first starting the game.
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void Pause()
	{
		Engine.TimeScale = 0;

		GetTree().Paused = true;
	}

	// Sets 'paused' to false and hides the pause-menu
	public void Resume()
	{
		Engine.TimeScale = 1;

		GD.Print("resume called");
		GetTree().Paused = false;
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
