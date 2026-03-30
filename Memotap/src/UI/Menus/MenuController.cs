using Godot;
using System;

public partial class MenuController : Node
{
	// ---- Node references ----
	[ExportGroup("Node references")]

	[Export] Control _pauseMenu;
	[Export] ColorRect _gameOverMenu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// make sure the pause menu or gameover menu is not visible when first starting the game.
		HideAll();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void Pause()
	{
		Engine.TimeScale = 0;

		GetTree().Paused = true;
		_pauseMenu.Visible = true;
	}

	// Sets 'paused' to false and hides the pause-menu
	public void Resume()
	{
		Engine.TimeScale = 1;

		GD.Print("resume called");
		GetTree().Paused = false;
		HideAll();
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

	private void GameOver()
	{
		_gameOverMenu.Visible = true;
	}

	private void HideAll()
	{
		_pauseMenu.Visible = false;
		_gameOverMenu.Visible = false;
	}
}
