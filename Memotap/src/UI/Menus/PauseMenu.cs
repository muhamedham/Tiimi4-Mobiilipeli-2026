using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// make sure the pause menu or gameover menu is not visible when first starting the game.
		Hide();
	}

	// Called when PauseButton is pressed
	public void OnPausePressed()
	{
		GetTree().Paused = true;
		Show();
	}

	// Sets 'paused' to false and hides the pause-menu
	public void OnResumePressed()
	{
		GetTree().Paused = false;
		Hide();
	}

	// Called when RestartButton is pressed
	private void OnRestartPressed()
	{
		GetTree().Paused = false;
		Hide();
		GetTree().ReloadCurrentScene();
	}

	// Called when QuitButton is pressed
	private void OnQuitPressed()
	{
		GetTree().Paused = false;
		Hide();
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}
}
