using Godot;
using System;

using Godot.Collections;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;


public partial class Game : Node2D
{

	[ExportGroup("Node references")]
	[Export] GoStopTexture _goStopTexture = null;
	[Export] HeartField _heartField = null;
	[Export] TileField _tileField = null;
	[Export] Score _score = null;
	//[Export] private Array<TileButton> _buttons = null;

	private Array <TileButton> _buttons = new();
	private Array <HeartTexture> _hearts = new();

	// Tasolaskuri- komponentti


	private Array<TileButton> _rndButtons = new();

	//tracks how far we are into the current level
	private int _index = 0;

	// current level of the game (how many buttons to show)
	[Export] private int _level = 1;

	[Export] private int _maxHealth = 3;
	[Export] public int _btnCount = 9;
	private int _lives;

	public int Lives
	{
		get {return _lives; }
		set
		{
			_lives = Mathf.Clamp(value, 0, _maxHealth);
		}
	}

	//List of all found level fileNames
	private Array<string> _levelNames = null;

	[Export] private int _levelLength = 3; 


	[ExportGroup("Timers")]
	[Export] public float _flashDuration = 0.75f;
	[Export] public float _nextRoundDelay = 2.0f;

	// Called when the node enters the scene tree for the first time.
	public async override void _EnterTree()
	{

		LoadLevels();

		Lives = _maxHealth;
		if (_heartField == null)
		{
			GD.PushError("_heartField node not found");
		} else
		{
			_hearts = _heartField.Setup(this);
		}

		if (_tileField == null)
		{
			GD.PushError("_tileField node not found");
		} else
		{
			_buttons = _tileField.Setup(this);
		}

		//loop through _buttons arr and start listening to each signal.
		foreach (TileButton button in _buttons)
		{
			button.CorrectPress += CorrectPressed;
			button.WrongPress += WrongPressed;
		}
		
		await ToSignal(GetTree().CreateTimer(_nextRoundDelay), "timeout");
		BuildSequence();
		ShowButtons();
	}

	public override void _ExitTree()
	{
		// stop listening to the signals
		foreach (var item in _buttons)
		{
			item.CorrectPress -= CorrectPressed;
			item.WrongPress -= WrongPressed;
		}
	}


	//TODO: do something when the wrong button has been pressed
	public async void WrongPressed()
	{
		_goStopTexture.SetState(Indicator.TileState.Active);
		Lives--;
		_index = 0;

		if (Lives <= 0)
		{	// wait until the end of the frame
			CallDeferred(MethodName.GameOver);
		}

		_hearts[Lives].SetState(Indicator.TileState.Inactive);


		//show the curent level buttons again.
		await ToSignal(GetTree().CreateTimer(_nextRoundDelay), "timeout");
		ShowButtons();
		_rndButtons[_index].SetIsCorrect(true);
	}


	public async void CorrectPressed()
	{
		GD.Print("Correct button Pressed");
		// set the button that was just pressed as not correct

		_rndButtons[_index].SetIsCorrect(false);
		_index++;

		// go into the next level
		if (_index >= _level)
			{
				_goStopTexture.SetState(Indicator.TileState.Active);
				_index = 0;
				_level ++;
				_score.SetText(_level);
				BuildSequence();
				await ToSignal(GetTree().CreateTimer(_nextRoundDelay), "timeout");
				ShowButtons();
			}

		_rndButtons[_index].SetIsCorrect(true);
	}

	private void BuildSequence()
	{
		int[][] data = SequenceLoader.LoadSequences(_levelNames[_level - 1]);


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
		//goStopTexture.SetSize();

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
			//how long to show each button as green before turning back
			await ToSignal(GetTree().CreateTimer(_flashDuration), "timeout");
			_rndButtons[i].Reset();
			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");

		}

		// Enable all buttons
		for (int i = 0; i < _buttons.Count; i++)
		{
			_buttons[i].ChangeDisable();
		}


		_goStopTexture.SetState(Indicator.TileState.Inactive);

		//goStopTexture.SetSize();

	}

	private void GameOver()
	{
		//TODO: do something when the game is over
			_index = 0;
			GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");

	}

	// Takes an entire level read straight from the file and returns a scrambled one with
	// the the amount of items defined by '_levelLength'
	// WORKING PROGRESS!!
	private static int[] LevelScrambler(int[] lvl)
	{
		int[] scrambled = [];

		return scrambled;
	}

	// WORKING PROGRESS!!
	private void LoadLevels()
	{
		_levelNames = SequenceLoader.GetAvailableLevels();
		GD.Print(_levelNames);
	}
}
