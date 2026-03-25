using Godot;
using System;

public partial class GameOverMenu : ColorRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Hide();
	}

	private void OnButtonPressed()
	{
		GD.Print("i hvae been pressed");
	}

	private void OnRestartPressed()
	{
		GD.Print("Restart pressed");
	}

	private void OnContinuePressed()
	{
		
	}
}
