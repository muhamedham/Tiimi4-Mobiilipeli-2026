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

	[ExportGroup("Index")]
	[Export] public int index = 0;

	[ExportGroup("My Textures")]
	[Export] public Texture2D DefaultTexture;
	[Export] public Texture2D RightTexture;
	[Export] public Texture2D WrongTexture;

	private TileState CurrentState = TileState.Default;

	[ExportGroup("Timer")]
	[Export] public float TimeLimit = 3.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		GD.Print("I have been pressed");
		SetState(TileState.Right);

		UpDateVisual();
		StartResetTimer();
	}


	public void SetState(TileState newState)
	{
		CurrentState = newState;
		UpDateVisual();
	}

	private void UpDateVisual()
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
		SetState(TileState.Default);
	}


}
