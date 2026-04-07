using Godot;
using System;
using System.Runtime.Serialization;

public partial class MainMenu : Node2D
{
	private AnimationPlayer _transition = null; 

    // Called when scene enters tree
    public override void _Ready()
    {
		// Assign transition node and play the transition
		_transition = GetNode<AnimationPlayer>("TransitionLayer/Transition");
		_transition.Play("fade-in");
		GD.Print("animation played");
    }

	// Called when 'PlayButton' is pressed
	private void OnPlayPressed()
	{
		// Playe the transition, functionality is handled after animation is finished
		_transition.Play("fade-out");
	}

	// Called when 'QuitButton' is pressed
	private void OnQuitPressed()
	{
		// Play transition (does not play fully, delegation not implemented	)
		_transition.Play("fade-out");
		GetTree().Quit();
	}	

	// Called when the transition node is done playing an animation.
	// should follow up with required functionality
	private void OnTransitionFinished(StringName animName)
	{
		if (animName == "fade-out")
		GetTree().ChangeSceneToFile("res://scenes/Game.tscn");
	}
}
