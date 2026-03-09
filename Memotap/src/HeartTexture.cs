using Godot;
using System;

public partial class HeartTexture : TextureRect
{

		public enum TileState
	{
		Default,
		Used,
	}

	private TileState CurrentState = TileState.Default;

	[ExportGroup("My Textures")]
	[Export] public Texture2D DefaultTexture;
	[Export] public Texture2D UsedTexture;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetState(TileState.Used);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetState(TileState newState)
	{
		CurrentState = newState;
	}

	public void UpDateVisual()
	{
		GD.Print("UpdateVisual Callded.");
		switch (CurrentState)
		{
			case TileState.Default:
				Texture = DefaultTexture;
				break;
			case TileState.Used:
				Texture = UsedTexture;
				break;
		}
	}

}
