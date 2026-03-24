using Godot;
using System;

public partial class RestartButton : Button
{
	[Signal]
	public delegate void RestartRequestEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonUp += OnPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void OnPressed()
	{
		EmitSignal(SignalName.RestartRequest);
	}
}
