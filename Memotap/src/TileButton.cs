using Godot;
using System;
using System.Data;
using System.Reflection.Metadata;

public partial class TileButton : Godot.TextureButton
{

	// ---- TileState ----
	public enum TileState
	{
		Default,
		Wrong,
		Right
	}

	// ---- Textures ----
	[ExportGroup("My Textures")]
	[Export] public Texture2D DefaultTexture;
	[Export] public Texture2D RightTexture;
	[Export] public Texture2D WrongTexture;

	// ---- Signals ----
	[Signal] public delegate void CorrectPressEventHandler();
	[Signal] public delegate void WrongPressEventHandler();


	// ---- State variables ----
	private bool _isCorrect;
	public bool IsCorrect
	{
		get => _isCorrect;
		set
		{
			_isCorrect = value;
		}
	}

	private TileState _currentState = TileState.Default;
	

	// ---- Godot Tree ----
 
	// Triggers when node enters scene 
	public override void _EnterTree()
    {
        Pressed += OnPressed;
    }

	// Triggers when node exits scene
    public override void _ExitTree()
    {
        Pressed -= OnPressed;
    }


	// ---- Signal sender ----

	// Sends signal depending on if button is wrong or right
	private async void OnPressed()
	{

		if (_isCorrect)
		{
			EmitSignal(SignalName.CorrectPress);
		}
		else
		{
			EmitSignal(SignalName.WrongPress);
		}
	}


	// ---- Tilestate management ----
	
	// Update the buttons current texture according to state
	public void UpDateVisual()
	{
		switch (_currentState)
		{
			case TileState.Default:
				TextureNormal = DefaultTexture;
				break;
			case TileState.Right:
				TextureNormal = RightTexture;
				break;
			case TileState.Wrong:
				TextureNormal = WrongTexture;
				break;
		}
	}


	// Set button to 'Right'-state
	public void SetGreen()
	{
		_currentState = TileState.Right;
		UpDateVisual();
	}

	// Set button to 'Wrong'-state
	public void SetRed()
	{
		_currentState = TileState.Wrong;
		UpDateVisual();
	}

	// Set button to 'Default'-state
	public async void Reset()
	{
		_currentState = TileState.Default;
		this.UpDateVisual();
	}
}
