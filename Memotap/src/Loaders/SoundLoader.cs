using Godot;
using Godot.Collections;
using System;

public partial class SoundLoader : Node
{

    //TODO: Stop listening to all signals!!!!!!!!!!!!!!!!!!!!!!!!!!

    public static SoundLoader Instance
    {
        get;
        private set;
    }

    public enum Sounds
    {
        Correct,
        Wrong,
        GameOver,
        Click
    }

    int _masterBusIndex = -1;
    int _musicBusIndex = -1;
    int _SFXBusIndex = -1;

    //define the sounds to play
    Array<AudioStream> audioStreams;

    //make an array to the actual players
    Array<AudioStreamPlayer> audioStreamPlayers = new();

    AudioStreamPlayer songPlayer;

    Label _soundLabel = null;

    // make sure there is only one instance of the soundloader
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            QueueFree();
            return;
        }

        //setup the music to start playig automatically
        songPlayer = new AudioStreamPlayer
        {
            Stream =  GD.Load<AudioStream>("res://art/Sounds/MemotapTaustaMusiikki.mp3"),
            Bus = "Music",
            Autoplay = true
        };
        AddChild(songPlayer);

        //load all the audio streams we want to play
        audioStreams =
        [
            GD.Load<AudioStream>("res://art/Sounds/klikkaus.mp3"),
            GD.Load<AudioStream>("res://art/Sounds/wronganswer.mp3"),
            GD.Load<AudioStream>("res://art/Sounds/gameover.mp3"),
            GD.Load<AudioStream>("res://art/Sounds/Correct.wav")

        ];

        //make as many sound players as we have sounds
        audioStreamPlayers.Resize(audioStreams.Count);

        // add audioplayers to all the sounds we have
        for (int i = 0; i< audioStreams.Count; i++)
        {
            audioStreamPlayers[i] = new AudioStreamPlayer
            {   //how many sound one player can play at once
                MaxPolyphony = 5,

                //what the player will play when called
                Stream = audioStreams[i],

                Bus = "SFX"
            };
            AddChild(audioStreamPlayers[i]);
        }

        //listen to scene changes to find all children
        GetTree().SceneChanged += () => FindChildren(GetParent());

        FindChildren(GetParent());

        _masterBusIndex = AudioServer.GetBusIndex("Master");
        _musicBusIndex = AudioServer.GetBusIndex("Music");
        _SFXBusIndex = AudioServer.GetBusIndex("SFX");

    }

    public void RegisterTileButtons(Array<TileButton> arr)
    {
        // the game has started so lower the volume of the background song
        AudioServer.SetBusVolumeDb(_musicBusIndex,-30f);

        foreach (TileButton button in arr)
        {
            button.CorrectPress += () => PlaySound(Sounds.Correct);
            button.WrongPress += () => PlaySound(Sounds.Wrong);
        }

    }
    private void PlaySound(Sounds sound)
    {
        switch (sound)
        {
            case Sounds.Correct:
                audioStreamPlayers[0].Play();
                break;
            case Sounds.Wrong:
                audioStreamPlayers[1].Play();
                break;
            case Sounds.GameOver:
                audioStreamPlayers[2].Play();
                break;
            case Sounds.Click:
                audioStreamPlayers[3].Play();
                break;
        }
    }


    private void ToggleMute(bool toggled)
    {
        AudioServer.SetBusMute(_masterBusIndex, toggled);

        ChangeSoundLabel();
    }

    private void ChangeSoundLabel()
    {
        // if we have not found a sound label can't do anything
        if(_soundLabel == null)
        {
            return;
        }

        if (AudioServer.IsBusMute(_masterBusIndex))
        {
            _soundLabel.Text = Tr("SOUNDOFF");
        }
        else
        {
            _soundLabel.Text = Tr("SOUNDON");
        }
    }

    // find children if it is a button start listening to its signals
    private void FindChildren(Node parent)
    {
        foreach (Node child in parent.GetChildren())
        {

            if (child is Button btn && child is not TileButton)
            {
                btn.Pressed += () => PlaySound(Sounds.Click);
            } else if (child is Game game)
            {
                game.GameOverSignal += () => PlaySound(Sounds.GameOver);
                game.ButtonShownSignal += () => PlaySound(Sounds.Correct);
            } else if (child is MuteButton muteBtn)
            {
                muteBtn.Toggled += ToggleMute;

                muteBtn.SetPressedNoSignal(AudioServer.IsBusMute(_masterBusIndex));

            } else if (child is Label soundLabel && child.Name == "SoundLabel")
            {
                _soundLabel = soundLabel;
                ChangeSoundLabel();
            }

            // call recursively to find all children
            FindChildren(child);
        }
    }
}

//tutorial:
// https://www.youtube.com/live/QgBecUl_lFs?si=ypeHbF3yeWI40pTe&t=1295
