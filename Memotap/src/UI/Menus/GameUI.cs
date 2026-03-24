using Godot;
using System;

public partial class GameUI : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PauseButton pauseButton = GetNode<PauseButton>("PauseButton");
		PauseMenu pauseMenu = GetNode<PauseMenu>("PauseMenu");

		pauseButton.PauseRequest += pauseMenu.Pause;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
