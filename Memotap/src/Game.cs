using Godot;
using System;

using Godot.Collections;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;


public partial class Game : Node2D
{

	// ---- Node references ----
	[ExportGroup("Node references")]
	[Export] private GoStopTexture _goStopTexture = null;
	[Export] private HeartField _heartField = null;
	[Export] private TileField _tileField = null;
	[Export] private Score _score = null;

	// ---- Timers ----
	[ExportGroup("Timers")]
	[Export] public float _flashDuration = 0.75f;
	[Export] public float _betweenFlashDuration = 0.5f;
	[Export] public float _nextRoundDelay = 2.0f;
	[Export] public float _wrongPressDelay = 2.0f;

	// ---- Tuning ----
	[ExportGroup("Tuning")]
	[Export] private int _level = 1;
	[Export] private int _maxHealth = 3;
	[Export] public int _btnCount = 9;
	[Export] private int _levelLength = 3;

	// ---- Runtime state ----
	private int _lives;
	private Array<TileButton> _buttons = new();
	private Array<TileButton> _rndButtons = new();
	private Array<HeartTexture> _hearts = new();

	// ---- Properties ----
	public int Lives
	{
		get { return _lives; }
		set { _lives = Mathf.Clamp(value, 0, _maxHealth); }
	}

	// ---- Level Management ---- 
	private int _currentLevel = 0;
	private Array<string> _levelNames = null;
	private int[][] _levelSequences = null;
	private int _currentlevelIndex = 0;
	private int _currentSequenceIndex = 0;
	private int[] _activeSequence = [];


	// Called when the node enters the scene tree for the first time.
	public async override void _EnterTree()
	{
		
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
		
		_levelNames = SequenceLoader.GetAvailableLevels();
		LoadLevel();
		await Timer(_nextRoundDelay);
		PlaySequence();
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

	// Function that triggers upon receiving signal from button.
	// Disables buttons to stop the user from 'spamming' button and
	// losing unnecessary hearts.
	// Signal is fired and function calls whenever user presses button with
	// '_isCorrect' == false.
	// handles heart loss and triggers the replay of sequence.
	//
	// TODO: a resetting mechanic where the wrongPressed resets you 
	// back to a certain point
	public void WrongPressed()
	{
		// Disable all buttons to stop player from pressing any other
		// buttons and potentially losing any additional lives
		SetAllButtonsDisabled(true);

		// Trigger indicator and reset sequenceIndex, also remove life
		_goStopTexture.SetState(Indicator.TileState.Active);
		Lives--;
		_currentSequenceIndex = 0;

		// Called for handling GameOver calls and heart updates aswell as
		// Replaying sequence
		HandleWrongPress();
	}

	// Gets called upon signal from a button being pressed with 
	// '_isCorrect' == true. Sets own correct as false and calls handler.
	//
	//TODO: There is a slight window where if you repress at the right time,
	// you can lose life.
	public void CorrectPressed()
	{
		// set the button that was just pressed as not correct,
		// update index
		_buttons[_activeSequence[_currentSequenceIndex]].SetIsCorrect(false);
		_currentSequenceIndex++;

		// call handler for processing followup
		HandleCorrectPress();
	}

	//TODO: Some kind of GAMEOVER screen or setback mechanic (or both! 
	// menu that gives both as options?)
	private void GameOver()
	{
		//TODO: do something when the game is over
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}

	// Handler for 'WrongPressed', updates hearts and calls 'GameOver'
	// when lives go to 0
	private async void HandleWrongPress()
	{
		// Sets heart of array correspondent to amount of lives as inactive
		_hearts[_lives].SetState(HeartTexture.TileState.Inactive);
		
		// Checks wether to end game or replay sequence
		if (_lives <= 0)
		{
			GameOver();
		} else
		{
			await Timer(_wrongPressDelay);
			PlaySequence();
		}
	}

	// Handler function for 'CorrectPressed' deals with potentially switching
	// to next sequence, level, or just setting next sequence button as correct
	private async void HandleCorrectPress()
	{
		// Checks if current sequence is over
		if (_currentSequenceIndex >= _activeSequence.Length)
		{
			// Activate stop-indicator
			_goStopTexture.SetState(Indicator.TileState.Active);

			// reset local index and update LevelIndex
			_currentSequenceIndex = 0;
			_currentlevelIndex++;

			// Check if upon updating LevelIndex its time to level up
			if (_currentlevelIndex >= _levelSequences.Length)
			{
				// Resets LevelIndex and updates level
				_currentlevelIndex = 0;
				_currentLevel++;

				// Update scoreboard
				//TODO: update to switch between sprites
				_score.SetText(_currentLevel + 1);

				// Load the next levels sequences
				LoadLevel();
			}

			// In both cases set timer and go to next round. 
			await Timer(_nextRoundDelay);
			PlaySequence();
		} 
		else // if sequence is not over, simply update next correct button
		{
			_buttons[_activeSequence[_currentSequenceIndex]].SetIsCorrect(true);
		}
	}


	// Loads and sets up the active level, called once per level 
	private void LoadLevel()
	{
		int[][] completeLevel = SequenceLoader.LoadSequences(_levelNames[_currentLevel]);
		_levelSequences = ShuffleLevel(completeLevel);
	}

	// Takes an entire level read straight from the file and returns a scrambled one with
	// the the amount of items defined by '_levelLength'
	// the parameter int[][]
	// WORKING PROGRESS!!
	private int[][] ShuffleLevel(int[][] lvl)
	{
		// checking that the level given has 
		if (lvl.Length < _levelLength)
		{
			GD.Print($"Level must have at least {_levelLength} items, Returning array.");
			return lvl;
		}

		// Clones the 2D array with 'Shallow clone' meaning it clones the outer layer
		// but the inner arrays remain as references to the originals.
		int[][] shuffled = (int[][])lvl.Clone();

		// The Fisher-Yates method of shuffling
		for (int i = shuffled.Length - 1; i > 0; i--)
		{
			int j = (int)(GD.Randi() % (uint)(i + 1));
			(shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
		}

		// This syntax makes it so that the list cuts off at desired length
		return shuffled[.._levelLength];
	}

	// Responsible for playing the sequence to the player
	private async void PlaySequence()
	{
		SetAllButtonsDisabled(true);

		// Update indicator to active and set up the sequence loop
		// by fetching the current sequence from the shuffled sequences
		_goStopTexture.SetState(Indicator.TileState.Active);
 		_activeSequence = _levelSequences[_currentlevelIndex];

		// Iterate through the active sequence
		for (int i = 0; i < _activeSequence.Length; i++)
		{
			TileButton btn = _buttons[_activeSequence[i]];
			btn.SetGreen();
			await Timer(_flashDuration);
			btn.Reset();
			await Timer(_betweenFlashDuration);
		}

		// Update the indicator and set correct input
		_currentSequenceIndex = 0;
		_buttons[_activeSequence[_currentSequenceIndex]].SetIsCorrect(true);
		_goStopTexture.SetState(Indicator.TileState.Inactive);
		
		// Correct inputs should be setup by now and inputs get unlocked
		SetAllButtonsDisabled(false);
	}

	// Helper function that disables all buttons, blocking input
	private void SetAllButtonsDisabled(bool b)
	{
		for (int i = 0; i < _buttons.Count(); i++)
			_buttons[i].SetDisabled(b);
	}

	// Helper function that acts as a timer
	private SignalAwaiter Timer(float seconds) => 
		ToSignal(GetTree().CreateTimer(seconds),"timeout");
}
