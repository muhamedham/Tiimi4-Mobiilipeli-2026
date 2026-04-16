// restarting from the same level multiple times does not work
using Godot;
using System;

using Godot.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;


public partial class Game : Node2D
{


	//  ---- Signals ----
		//game over signal emitted when the game is over
	[Signal] public delegate void GameOverSignalEventHandler();

		// emit a signal when a button is shown
	[Signal] public delegate void ButtonShownSignalEventHandler();

	[Signal] public delegate void OnLevelPassedSignalEventHandler();

	// ---- Node references ----
	[ExportGroup("Node references")]
	[Export] private GoStopTexture _goStopTexture = null;
	[Export] private HeartField _heartField = null;
	[Export] private TileField _tileField = null;
	[Export] private Score _score = null;
	private AnimationPlayer _transition = null;


	// ---- Timers ----
	[ExportGroup("Timers")]
	[Export] public float _flashDuration = 0.75f;
	[Export] public float _pressFlashDuration = 0.5f;
	[Export] public float _betweenFlashDuration = 0.5f;
	[Export] public float _nextRoundDelay = 1.0f;
	[Export] public float _wrongPressDelay = 2.0f;

	// ---- Tuning ----
	[ExportGroup("Tuning")]
	[Export] private int _maxHealth = 3;
	[Export] public int _btnCount = 9;
	[Export] private int _levelLength = 3;

	// ---- Runtime state ----
	private int _lives;
	private Array<TileButton> _buttons = new();
	private Array<HeartTexture> _hearts = new();

	// ---- Properties ----
	public int Lives
	{
		get { return _lives; }
		set { _lives = Mathf.Clamp(value, 0, _maxHealth); }
	}

	// ---- Level Management ----
	public int _currentLevel = 0;
	private Array<string> _levelNames = null;
	private int[][] _levelSequences = null;
	private int[][] _lastLevelSequences = null;
	public int _currentlevelIndex = 0;
	private int _currentSequenceIndex = 0;
	private int[] _activeSequence = [];
	private TileButton _correctButton = null;


	// ---- GODOT MANAGEMENT ----

	// Called when the node enters the scene tree for the first time.
	public async override void _EnterTree()
    {


        // Check that all elements were found
        Lives = _maxHealth;
        if (_heartField == null)
        {
            GD.PushError("_heartField node not found");
        }
        else
        {
            _hearts = _heartField.Setup(this);
        }

        if (_tileField == null)
        {
            GD.PushError("_tileField node not found");
        }
        else
        {
            _buttons = _tileField.Setup(this);
        }

        _goStopTexture.GoStopTurn();

        // Disable inputs to avoid early disturbance
        SetAllButtonsDisabled(true);

        //loop through _buttons arr and start listening to each signal.
        foreach (TileButton button in _buttons)
        {
            button.CorrectPress += () => CorrectPressed(button);
            button.WrongPress += () => WrongPressed(button);
        }

        //give the buttons to soundloader to register sounds
        SoundLoader.Instance.RegisterTileButtons(_buttons);

        // Set the transition Node and play the transition
        _transition = GetNode<AnimationPlayer>("TransitionLayer/Transition");
        _transition.Play("fade-in");

        // Load level data
        _levelNames = SequenceLoader.GetAvailableLevels();
        LoadLevel();

        GD.Print("Viewport height: " + GetViewport().GetVisibleRect().Size.Y);

    }

	//we'll actually start the game when the tutorial is dismissed
    public async Task StartGame()
    {
        await Timer(_nextRoundDelay);
        PlaySequence();
    }

    public override void _ExitTree()
	{
		// stop listening to the signals
		foreach (var button in _buttons)
		{
			button.CorrectPress -= () => CorrectPressed(button);
			button.WrongPress -= () => WrongPressed(button);
		}
	}

	// ---- GameLoop Management ----

	// Responsible for playing the sequence to the player
	private async void PlaySequence()
	{
		// Update indicator to active and set up the sequence loop
		// by fetching the current sequence from the shuffled sequences
		_goStopTexture.SetState(Indicator.TileState.Active);
		_goStopTexture.GoStopTurn();
 		_activeSequence = _levelSequences[_currentlevelIndex];

		// Iterate through the active sequence
		for (int i = 0; i < _activeSequence.Length; i++)
		{
			TileButton btn = _buttons[_activeSequence[i]];
			EmitSignal(SignalName.ButtonShownSignal);
			btn.SetGreen();
			await Timer(_flashDuration);
			btn.Reset();
			await Timer(_betweenFlashDuration);
		}

		// Update the indicator and set correct input
		_currentSequenceIndex = 0;
		SetCorrectButton(_activeSequence[_currentSequenceIndex]);
		_goStopTexture.SetState(Indicator.TileState.Inactive);
		_goStopTexture.GoStopTurn();

		// Correct inputs should be setup by now and inputs get unlocked
		SetAllButtonsDisabled(false);
	}

	//TODO: Some kind of GAMEOVER screen or setback mechanic (or both!
	// menu that gives both as options?)
	private void GameOver()
	{
		EmitSignal(SignalName.GameOverSignal);
	}

	//store the last level to an array

	public async void HandleCheckpoint()
	{
		Lives = _maxHealth;
		if (_currentLevel != 0)
		{
			 _levelSequences = DupeArr(_lastLevelSequences);
			 _currentLevel --;
		}
		_currentlevelIndex = 0;

		UpdateScoreBoard();
		FillHearts();
		await Timer(_nextRoundDelay);
		PlaySequence();
	}


	// ---- Level management ----

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


	// ---- Input Handling ----

	// Function that triggers upon receiving signal from button.
	// Disables buttons to stop the user from 'spamming' button and
	// losing unnecessary hearts.
	// Signal is fired and function calls whenever user presses button with
	// '_isCorrect' == false.
	// handles heart loss and triggers the replay of sequence.
	//
	// TODO: a resetting mechanic where the wrongPressed resets you
	// back to a certain point
	public async Task WrongPressed(TileButton sender)
	{
		_ = ShowWrongButton(sender); // color the button accordingly

		// Disable all buttons to stop additional inputs during process
		SetAllButtonsDisabled(true);

		// Trigger indicator and reset sequenceIndex, also remove life
		_goStopTexture.SetState(Indicator.TileState.Active);
		_goStopTexture.GoStopTurn();
		Lives--;
		_currentSequenceIndex = 0;

		// Called for handling GameOver calls and heart updates aswell as
		// Replaying sequence
		HandleWrongPress();
	}

	// Gets called upon signal from a button being pressed with
	// '_isCorrect' == true. Sets own correct as false and calls handler.
	public async Task CorrectPressed(TileButton sender)
	{
		_ = ShowCorrectButton(sender); // color the button accordingly

		// Disable all buttons to stop additional input during the process
		SetAllButtonsDisabled(true);

		// set the button that was just pressed as not correct,
		// update index
		_currentSequenceIndex++;

		// call handler for processing followup
		HandleCorrectPress();
	}

	// Handler for 'WrongPressed', updates hearts and calls 'GameOver'
	// when lives go to 0
	private async void HandleWrongPress()
	{
		// Sets heart of array correspondent to amount of lives as inactive
		_hearts[Lives].SetState(Indicator.TileState.Inactive);

		// Checks wether to end game or replay sequence
		if (Lives <= 0)
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
			_goStopTexture.GoStopTurn();

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
                UpdateScoreBoard();

                _lastLevelSequences = DupeArr(_levelSequences);

				EmitSignal(SignalName.OnLevelPassedSignal);

                // Load the next levels sequences
                LoadLevel();
            }

            // In both cases set timer and go to next round.
            await Timer(_nextRoundDelay);
			PlaySequence();
		}
		else // if sequence is not over, simply update next correct button
		{
			SetCorrectButton(_activeSequence[_currentSequenceIndex]);
			SetAllButtonsDisabled(false);
		}
	}



    // ---- Button management ----

    // Responsible for coloring and resetting button
    private async Task ShowCorrectButton(TileButton Button)
	{
		Button.SetGreen();
		await Timer(_pressFlashDuration);
		Button.Reset();
	}

	// Responsible for coloring and resetting button
	private async Task ShowWrongButton(TileButton Button)
	{
		Button.SetRed();
		await Timer(_pressFlashDuration);
		Button.Reset();
	}

	// Sets reference for the correct button and enables correct within button
	// doublecheck exists so that no re-iteration of arrays is needed,
	// while allowing buttons to still send the correct or wrong signal
	private void SetCorrectButton(int index)
	{
		if (_correctButton != null)
			_correctButton.IsCorrect = false;

		_correctButton = _buttons[index];
		_correctButton.IsCorrect = true;
	}


	// ---- Helpers ----


	private void FillHearts()
	{
		foreach (Indicator heart in _hearts)
		{
			heart.SetState(Indicator.TileState.Active);
		}
	}

	//helper function to update the scoreboard
	private void UpdateScoreBoard()
    {
        _score.SetLevel(_currentLevel +1);
    }

	// Helper function that disables all buttons, blocking input
	private void SetAllButtonsDisabled(bool b)
	{
		for (int i = 0; i < _buttons.Count(); i++)
			_buttons[i].SetDisabled(b);
	}

	//helper function to deep copy an array
	private int[][] DupeArr(int[][] oldArr)
	{
		int[][] newArr = new int[oldArr.Length][];
		for (int i = 0; i < oldArr.Length; i++)
		{
			newArr[i] = new int[oldArr[i].Length];
		}
		for (int i = 0; i < oldArr.Length; i++)
		{
			for (int j = 0; j < oldArr[i].Length; j++)
			{
				newArr[i][j] = oldArr[i][j];
			}
		}
		return newArr;
	}

	// Helper function that acts as a timer
	private SignalAwaiter Timer(float seconds) =>
		ToSignal(GetTree().CreateTimer(seconds),"timeout");




}
