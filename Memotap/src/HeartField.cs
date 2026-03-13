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

		for (int i = 0; i < _hearts.Count; i++)
		{
			HeartTexture heart = _heartScene.Instantiate<HeartTexture>();
			_hearts[i] = heart;
			AddChild(heart);
		}
		return _hearts.Duplicate();
	}
}
