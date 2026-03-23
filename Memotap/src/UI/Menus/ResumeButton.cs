using Godot;
using System;

public partial class ResumeButton : Button
{

	[Signal]
	public delegate void ResumeRequestEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonUp += OnPressed;
	}

	public void OnPressed()
	{
		EmitSignal(SignalName.ResumeRequest);
		GD.Print("OnPressed called");
	}
}
