using Godot;
using System;
using Godot.Collections;

public partial class ButtonRunner : Node2D
{
	[Export] public Array<TileButton> _buttons = null;
	private Array<int> _indices = new();
	[Export] public int length = 5;
	private float _startTimer = 2.0f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StartTimer();
	}

	private async void StartTimer()
	{
		await ToSignal(GetTree().CreateTimer(_startTimer), "timeout");
		ChooseButtons();
	}

	private void ChooseButtons()
	{
		for (int i = 0; i < length; i++)
		{
			_indices[i] = (int)(GD.Randi() % 6);
			GD.Print(_indices[i]);

			_buttons[_indices[i]].SetState(TileButton.TileState.Right);
		}
	}
}
