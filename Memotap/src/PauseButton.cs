using Godot;
using System;

public partial class PauseButton : MenuButton
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonUp += OnPressed;
	}

	public void OnPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
