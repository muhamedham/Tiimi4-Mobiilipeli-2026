using Godot;
using System;

using Godot.Collections;

public partial class GameRunner : Node
{

	[Export] private Array<TileButton> _buttons = null;

	private Array<TileButton> _rndButtons = new();

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


	//TODO do something when the wrong button has been pressed
	public void WrongPressed()
	{
		GD.Print("Wrong button has been pressed, DO SOMETHING");
	}


	public async void CorrectPressed()
	{
		GD.Print("Correct button Pressed");
		// set the button that was just pressed as not correct
		_rndButtons[_index].SetIsCorrect(false);
		_index++;

		if (_index >= _level)
			{
				_index = 0;
				_level ++;
				await ToSignal(GetTree().CreateTimer(_timeLimit), "timeout");
				PickButtons();
				ShowButtons();
			}

		_rndButtons[_index].SetIsCorrect(true);
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

		// set the first button as the correct button
		_rndButtons[0].SetIsCorrect(true);
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

		// Enable all buttons
		for (int i = 0; i < _buttons.Count; i++)
		{
			_buttons[i].ChangeDisable();
		}
	}

}
