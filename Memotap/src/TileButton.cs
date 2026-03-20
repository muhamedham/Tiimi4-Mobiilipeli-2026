using Godot;
using System;

public partial class TileButton : Godot.TextureButton
{

	public enum TileState
	{
		Default,
		Wrong,
		Right
	}

	[ExportGroup("My Textures")]
	[Export] public Texture2D DefaultTexture;
	[Export] public Texture2D RightTexture;
	[Export] public Texture2D WrongTexture;

	private TileState CurrentState = TileState.Default;

	[ExportGroup("Timer")]
	[Export] public float TimeLimit = 0.5f;

	[Signal] public delegate void CorrectPressEventHandler();
	[Signal] public delegate void WrongPressEventHandler();


	private bool _correctBtn = false;
	private bool _isProcessing = false;

	public TileButton()
	{

	}

    public override void _EnterTree()
    {
        Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        Pressed -= OnPressed;
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// default buttons to wrong if they are pressed.
		SetState(TileState.Wrong);
	}

	private async void OnPressed()
	{
		GD.Print("Onpressed called");
		if (_isProcessing) 
		{
			GD.Print("Onpressed returns; processing");
			return;
		}

		_isProcessing = true;

		// if the tile state has not been set to chosen by gamerunner it's wrong
		if (_correctBtn)
		{
			SetState(TileState.Right);
			UpDateVisual();
			EmitSignal(SignalName.CorrectPress);

		} else
		{
			SetState(TileState.Wrong);
			UpDateVisual();
			EmitSignal(SignalName.WrongPress);
		}

		await ToSignal(GetTree().CreateTimer(TimeLimit), "timeout");
		Reset();
		_isProcessing = false;
		GD.Print("OnPressed return; Done");
	}

	public void SetState(TileState newState)
	{
		CurrentState = newState;
	}

	public void UpDateVisual()
	{
		switch (CurrentState)
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


	public void SetGreen()
	{
		GD.Print("Turn green called");
		this.SetState(TileState.Right);

		UpDateVisual();
	}

	public async void Reset()
	{
		// turn button to default and show it to the player.
		this.SetState(TileState.Default);
		this.UpDateVisual();
	}

	public void ChangeDisable()
	{
		if (this.Disabled)
		{
			this.Disabled = false;
			GD.Print("Button enabled");
			return;
		}

		if (!this.Disabled)
		{
			this.Disabled = true;
			GD.Print("button disabled");
			return;
		}
	}

	public void SetIsCorrect(bool isCorrrect)
	{
		this._correctBtn = isCorrrect;
	}
}
