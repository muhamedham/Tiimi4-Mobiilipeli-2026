using Godot;
using System;
using System.Threading.Tasks;

public partial class TileButton : Godot.TextureButton
{

	public enum TileState
	{
		Default,
		Wrong,
		Right
	}



	GameRunner gameRunner;

	[ExportGroup("My Textures")]
	[Export] public Texture2D DefaultTexture;
	[Export] public Texture2D RightTexture;
	[Export] public Texture2D WrongTexture;

	private TileState CurrentState = TileState.Default;

	[ExportGroup("Timer")]
	[Export] public float TimeLimit = 0.5f;


	private bool CorrectBtn = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameRunner = GetNode<GameRunner>("/root/Game/GameRunner");
		// default buttons to wrong if they are pressed.
		SetState(TileState.Wrong);
		Pressed += OnPressed;
	}

	private async void OnPressed()
	{
		GD.Print("I have been pressed");

		// if the tile state has not been set to chosen by gamerunner it's wrong
		if (CorrectBtn)
		{
			SetState(TileState.Right);
			UpDateVisual();
			StartResetTimer();
			gameRunner.CorrectPressed();

		} else
		{
			SetState(TileState.Wrong);
			UpDateVisual();
			StartResetTimer();
			gameRunner.WrongPressed();
		}

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

	public async void StartResetTimer()
	{
		await ToSignal(GetTree().CreateTimer(TimeLimit), "timeout");
		Reset();
	}

	public void SetGreen()
	{
		GD.Print("Turn green called");
		this.SetState(TileState.Right);
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
		this.CorrectBtn = isCorrrect;
	}
}
