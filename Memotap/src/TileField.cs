using Godot;
using System;

using Godot.Collections;

public partial class TileField : GridContainer
{
	[Export] private PackedScene _buttonScene = null;
	private Array<TileButton> _buttons = new();

	public Array<TileButton> Setup(Game gameRunner)
	{
		_buttons.Resize(gameRunner._btnCount);

		for (int i = 0; i < _buttons.Count; i++)
		{
			TileButton button = _buttonScene.Instantiate<TileButton>();
			_buttons[i] = button;
			AddChild(button);
		}
		return _buttons.Duplicate();
	}
}
