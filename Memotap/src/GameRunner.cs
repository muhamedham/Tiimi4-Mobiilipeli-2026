using Godot;
using System;

using Godot.Collections;

public partial class GameRunner : Node
{

	[Export] private Array<TileButton> _buttons = null;

	private Array<TileButton> _rndButtons = new();

	private bool _wasPressedLastFrame = false;
	private int _index = 0;

	private int _level = 3;

	[ExportGroup("Timer")]
	[Export] public float _timeLimit = 2.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PickButtons();

		ShowButtons();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		bool isCorrectPressedNow = _rndButtons[_index].ButtonPressed;

		if (isCorrectPressedNow && !_wasPressedLastFrame)
		{
			CorrectPressed();
		}

		_wasPressedLastFrame = isCorrectPressedNow;

		if (_index >= _level)
		{
			_index = 0;
			_level ++;

			PickButtons();
			ShowButtons();
		}
	}

	private void CorrectPressed()
	{
		GD.Print("Correct button Pressed");
		_rndButtons[_index].SetGreen();
		_index++;
	}

	private void PickButtons()
	{
		//empty the array if there is something in it.
		if (_rndButtons.Count != 0 )
		{
			_rndButtons.Clear();
		}
		//add buttons in random order to a new array
		for (int i = 0; i < _level; i++)
		{
			_rndButtons.Add(_buttons.PickRandom());
		}
	}

	public async void ShowButtons()
	{

		//Disable all buttons
		for (int i = 0; i < _buttons.Count; i++)
		{
			_buttons[i].ChangeDisable();
		}

		// loop through buttons turn them to green then turn back
		for (int i = 0; i < _rndButtons.Count; i++)
		{
			_rndButtons[i].SetGreen();
			_rndButtons[i].UpDateVisual();

			await ToSignal(GetTree().CreateTimer(_timeLimit), "timeout");
			_rndButtons[i].Reset();

		}

		// Disable all buttons
		for (int i = 0; i < _buttons.Count; i++)
		{
			_buttons[i].ChangeDisable();
		}
	}


}
