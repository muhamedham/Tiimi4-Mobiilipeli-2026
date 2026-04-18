
using Godot;
using System;
using System.ComponentModel;

public partial class GameOverMenu : ColorRect
{
    // ---- Node References ----
    [ExportGroup("Node References")]
    [Export] private Game _gameRunner = null;
    [Export] private Label _checkPointLabel = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

        _gameRunner.GameOverSignal += OnGameOver;
		// make sure the pause menu or gameover menu is not visible when first starting the game.
        Hide();
	}

    // Called when PauseButton is pressed
	private void OnGameOver()
    {
        Show();
        int level = -1;
        if (_gameRunner._currentLevel <= 0)
        {
            level = 1;
        } else
        {
            level = _gameRunner._currentLevel;
        }
        string localizedScore = Tr("LEVEL");
		_checkPointLabel.Text = string.Format(localizedScore,level);
    }


	// Sets 'paused' to false and hides the pause-menu
	public void OnContinuePressed()
	{
		GetTree().Paused = false;
        Hide();
        _gameRunner.HandleCheckpoint();
    }

    // Called when RestartButton is pressed
    private void OnRestartPressed()
    {
        GetTree().Paused = false;
        Hide();

        GetTree().ReloadCurrentScene();
    }

    // Called when QuitButton is pressed
    private void OnQuitPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/Menu.tscn");
    }
}