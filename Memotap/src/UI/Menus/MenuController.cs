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

	public void Pause()
	{
		Engine.TimeScale = 0;

		GetTree().Paused = true;
	}

	// Sets 'paused' to false and hides the pause-menu
	public void Resume()
	{
		Engine.TimeScale = 1;

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
		Resume();
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}
}
