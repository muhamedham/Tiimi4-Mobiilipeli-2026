using Godot;
using Godot.Collections;
using System;

public partial class SoundLoader : Node
{

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

    //define the sounds to play
    Array<AudioStream> audioStreams;

    //make an array to the actual players
    Array<AudioStreamPlayer> audioStreamPlayers = new();

    AudioStreamPlayer songPlayer;

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


        audioStreams =
        [
            GD.Load<AudioStream>("res://art/Sounds/klikkaus.mp3"),
            GD.Load<AudioStream>("res://art/Sounds/wronganswer.mp3"),
            GD.Load<AudioStream>("res://art/Sounds/gameover.mp3"),
            GD.Load<AudioStream>("res://art/Sounds/Correct.wav")

        ];

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

        GetTree().SceneChanged += () => FindChildren(GetParent());

        FindChildren(GetParent());
    }

    public void Setup(Array<TileButton> arr)
    {
        songPlayer.VolumeDb = -30f;

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


//Voisi käyttää jos nappeja ei enää lisätä koodissa vaan käsin
// koodi etsii itse napit ja kuuntelee painallus signaaleja
    private void FindChildren(Node parent)
    {
        foreach (Node child in parent.GetChildren())
        {
            if (child is Button btn && child is not TileButton)
            {
                GD.Print(child.Name + " has been found");
                btn.Pressed += () => PlaySound(Sounds.Click);
            } else if (child is Game game)
            {
                game.GameOverSignal += () => PlaySound(Sounds.GameOver);
                game.ButtonShownSignal += () => PlaySound(Sounds.Correct);
            }
            // call recursively to find all children
            FindChildren(child);
        }

    }


    //TODO? Lisää UI nappeihin myös äänet
    // ja silloin kun nappeja näytetään
}

//tutorial:
// https://www.youtube.com/live/QgBecUl_lFs?si=ypeHbF3yeWI40pTe&t=1295
