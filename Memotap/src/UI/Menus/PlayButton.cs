using Godot;
using System;

public partial class PlayButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonUp += OnRelease;
	}

	public void OnRelease()
	{
		GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	}
}
