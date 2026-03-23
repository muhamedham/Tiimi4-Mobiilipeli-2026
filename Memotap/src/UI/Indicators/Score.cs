using Godot;
using System;

public partial class Score : Godot.Label
{
	//
	public void SetText(int value)
	{
		Text = "LEVEL " + value.ToString();
	}
}
