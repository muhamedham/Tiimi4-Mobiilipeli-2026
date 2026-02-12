using Godot;
using System;

using Godot.Collections;

public partial class GameRunner : Node
{

	[Export] private Array<TileButton> _buttons = null;

	private Array<TileButton> _buttonArr = new();

	private bool _wasPressedLastFrame = false;
	private int index = 0;
	private float _timeLimit = 2.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		for (int i = 0; i < 5; i++)
		{
			_buttonArr.Add(_buttons.PickRandom());
		}

		ShowButtons();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		bool isPressedNow = _buttonArr[index].ButtonPressed;

		if (isPressedNow && !_wasPressedLastFrame)
		{
			Something();
		}

		_wasPressedLastFrame = isPressedNow;
	}

	private void Something()
	{
		GD.Print("Correct button Pressed");
		_buttonArr[index].SetGreen();
		index++;
	}

	public async void ShowButtons()
	{
		for (int i = 0; i < _buttonArr.Count; i++)
		{
			_buttonArr[i].SetGreen();
			_buttonArr[i].UpDateVisual();

			await ToSignal(GetTree().CreateTimer(_timeLimit), "timeout");
			_buttonArr[i].Reset();

		}
	}
}
