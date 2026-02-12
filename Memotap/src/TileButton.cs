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
	[Export] public float TimeLimit = 1.2f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetState(TileState.Wrong);
		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		GD.Print("I have been pressed");

		UpDateVisual();
		StartResetTimer();
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
	private async void StartResetTimer()
	{
		await ToSignal(GetTree().CreateTimer(TimeLimit), "timeout");
		Reset();

	}

	public void SetGreen()
	{
		GD.Print("Turn green called");
		this.SetState(TileState.Right);

	}

	public void Reset()
	{
		this.SetState(TileState.Default);
		this.UpDateVisual();
		this.SetState(TileState.Wrong);
	}
}
