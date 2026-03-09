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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void SetActive()
    {
        SetState(TileState.Active);
        UpDateVisual();
    }

    public void SetInactive()
    {
        SetState(TileState.Inactive);
        UpDateVisual();
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
			case TileState.Active:
				Texture = Activetexture;
				break;
			case TileState.Inactive:
				Texture = InactiveTexture;
				break;
		}
	}

}
