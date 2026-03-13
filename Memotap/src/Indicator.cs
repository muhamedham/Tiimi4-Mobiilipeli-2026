using Godot;
using System;

public partial class Indicator : TextureRect
{

		public enum TileState
	{
		Active,
		Inactive,
	}

	private TileState CurrentState = TileState.Active;

	[ExportGroup("My Textures")]
	[Export] public Texture2D Activetexture;
	[Export] public Texture2D InactiveTexture;

	
	public void SetState(TileState newState)
	{
		CurrentState = newState;
		UpDateVisual();
	}

	public void UpDateVisual()
	{
		GD.Print("UpdateVisual Callded.");
		switch (CurrentState)
		{
			case TileState.Active:
				Texture = Activetexture;
				break;
			case TileState.Inactive:
				Texture = InactiveTexture;
				break;
		}
	}

}
