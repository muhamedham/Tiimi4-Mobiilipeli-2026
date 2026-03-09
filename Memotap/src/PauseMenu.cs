using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.

	GameRunner gameRunner;
	public override void _Ready()
	{
		Visible = false;

		ResumeButton resumeButton = GetNode<ResumeButton>("PanelContainer/VBoxContainer/ResumeButton");
		RestartButton restartButton = GetNode<RestartButton>("PanelContainer/VBoxContainer/RestartButton");
		BackButton backButton = GetNode<BackButton>("PanelContainer/VBoxContainer/BackButton");

		gameRunner = GetNode<GameRunner>("/root/Game/GameRunner");

		resumeButton.ResumeRequest += Resume;
		restartButton.RestartRequest += Restart;
		backButton.BackRequest += Quit;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Pauses the game and shows the pause-menu
	public void Pause()
	{

		gameRunner.OnPausePressed();

		GetTree().Paused = true;
		Visible = true;
	}

	// Sets 'paused' to false and hides the pause-menu
	public void Resume()
	{

		gameRunner.OnResumePressed();

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

	public void Quit()
	{
		//mystistä
		Resume();
		GD.Print("quit FUNC Called");
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}
}
