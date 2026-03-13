using Godot;
using System;

using Godot.Collections;


public partial class Game : Node2D
{
	GoStopTexture goStopTexture;
	HeartField heartField;
	TileField tileField;
	Score _score;

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


	[ExportGroup("Timers")]
	[Export] public float _flashDuration = 0.75f;
	[Export] public float _nextRoundDelay = 2.0f;



	// Called when the node enters the scene tree for the first time.
    public async override void _EnterTree()
    {

		Lives = _maxHealth;

		/*
		Ready metodissa voisi määritellä kaikki aiemmin määritellyt
		'heart', 'score' ja 'button'.

		jokainen määriteltäisiin for-loopissa Getnode- metodilla kunnes ei enään löydy elementtiä

		*/

		_score = GetNode<Score>("/root/Game/GameUI/Score");

		goStopTexture = GetNode<GoStopTexture>("/root/Game/GameUI/GoStopTexture");

		heartField = GetNode<HeartField>("/root/Game/HeartField");
		_hearts = heartField.Setup(this);


		tileField = GetNode<TileField>("/root/Game/TileField");
		_buttons = tileField.Setup(this);

		foreach (var item in _buttons)
		{
			item.CorrectPress += CorrectPressed;
			item.WrongPress += WrongPressed;
		}

		_score.SetText(_level);


		await ToSignal(GetTree().CreateTimer(_nextRoundDelay), "timeout");
		PickButtons();
		ShowButtons();
    }

    public override void _ExitTree()
    {
		foreach (var item in _buttons)
		{
			item.CorrectPress -= CorrectPressed;
			item.WrongPress -= WrongPressed;
		}
    }


	//TODO: do something when the wrong button has been pressed
	public async void WrongPressed()
	{
		goStopTexture.SetState(Indicator.TileState.Active);
		_lives --;
		_index = 0;

		if (Lives == -1)
		{
			GameOver();
		}

		if (_lives > -1) _hearts[_lives].SetState(Indicator.TileState.Inactive);

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
				goStopTexture.SetState(Indicator.TileState.Active);
				_index = 0;
				_level ++;
				_score.SetText(_level);
				PickButtons();
				await ToSignal(GetTree().CreateTimer(_nextRoundDelay), "timeout");
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


		goStopTexture.SetState(Indicator.TileState.Inactive);

		//goStopTexture.SetSize();

	}

	private void GameOver()
	{
		//TODO: do something when the game is over
			_index = 0;
			GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");

	}
}
