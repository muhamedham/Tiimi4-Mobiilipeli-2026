using Godot;
using System;

public partial class TextureButton2 : TextureButton
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

	private int _activeTouchIndex = -1;

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touch)
		{
			if (touch.Pressed)
			{
				_activeTouchIndex = touch.Index;
				GD.Print("Pressed down inside");
			}
			else
			{
				if (touch.Index == _activeTouchIndex)
				{
					bool releasedInside = GetRect().HasPoint(touch.Position);

					if (releasedInside)
					{
						GD.Print("Released inside button");
					}
					else
					{
						GD.Print("Released outside button");
					}
					_activeTouchIndex = -1;
				}
			}
		}
		else
		{
			GD.Print(@event);
		}
    }
}
