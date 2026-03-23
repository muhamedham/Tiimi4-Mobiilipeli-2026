using Godot;
using System;

using Godot.Collections;

public partial class HeartField : HBoxContainer
{

	[Export] private PackedScene _heartScene = null;
	private Array <HeartTexture> _hearts = new();

	public Array<HeartTexture> Setup(Game gameRunner)
	{
		_hearts.Resize(gameRunner.Lives);

		// iterate down so that heart references end up being from right to left
		// (makes losing and gaining hearts simpler)
		for (int i = _hearts.Count - 1; i >= 0; i--)
		{
			HeartTexture heart = _heartScene.Instantiate<HeartTexture>();
			_hearts[i] = heart;
			AddChild(heart);
		}
		return _hearts.Duplicate();
	}
}
