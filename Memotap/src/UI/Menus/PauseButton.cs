using Godot;
using System;
using System.ComponentModel;

public partial class PauseButton : TextureButton
{

	[Signal]
	public delegate void PauseRequestEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonUp += OnPressed;
	}

	public void OnPressed()
	{
		EmitSignal(SignalName.PauseRequest);
	}
}
