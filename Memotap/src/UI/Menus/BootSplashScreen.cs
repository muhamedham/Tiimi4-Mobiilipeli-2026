using Godot;
using System;

public partial class BootSplashScreen : Node2D
{
	private void SplashScreenTimeout()
	{
		GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
	}
}

