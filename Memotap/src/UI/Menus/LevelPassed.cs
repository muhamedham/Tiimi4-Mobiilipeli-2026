using Godot;
using System;

public partial class LevelPassed : Control
{
    [Export] Game _game;

	[Export] CpuParticles2D _particleEffect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// make sure the level passed menu is not visible when first starting the game.

	}

    public override void _EnterTree()
    {
        Hide();
        _game.OnLevelPassedSignal += OnLevelCompletion;
    }
	// Called when pausing between levels
	public void OnLevelCompletion()
	{
		_particleEffect.Emitting = true;
		var timer = GetTree().CreateTimer(0.5f, false);
        timer.Timeout += () =>
        {

            GetTree().Paused = true;
			_particleEffect.Emitting = true;
            Show();
        };
    }

	// Sets 'paused' to false and hides the pause-menu
	public void OnResumePressed()
	{
		GetTree().Paused = false;
		Hide();
	}
}
