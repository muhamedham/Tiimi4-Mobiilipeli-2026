using Godot;
using Godot.Collections;
using System;

public partial class SoundLoader : Node
{
    //define the sounds to play
    [Export] Array<AudioStream> audioStreams;

    //make an array to the actual players
    Array<AudioStreamPlayer> audioStreamPlayers = new();

    public void Setup(Array<TileButton> arr)
    {
        audioStreamPlayers.Resize(audioStreams.Count);

        // add audioplayers to all the sounds we have
        for (int i = 0; i< audioStreams.Count; i++)
        {
            audioStreamPlayers[i] = new AudioStreamPlayer
            {   //how many sound one player can play at once
                MaxPolyphony = 5,

                //what the player will play when called
                Stream = audioStreams[i]
            };
            AddChild(audioStreamPlayers[i]);
        }

        foreach (TileButton button in arr)
        {
            button.CorrectPress += () => PlaySound(audioStreamPlayers[0]);
            button.WrongPress += () => PlaySound(audioStreamPlayers[1]);
        }

        Node node = GetParent();
        CallDeferred(MethodName.FindChildren, node);

    }
    private void PlaySound(AudioStreamPlayer player)
    {
        player.Play();
    }


//Voisi käyttää jos nappeja ei enää lisätä koodissa vaan käsin
// koodi etsii itse napit ja kuuntelee painallus signaaleja
    private void FindChildren(Node node)
    {
        foreach (Node child in node.GetChildren())
        {

            if (child is Button btn && child is not TileButton)
            {
                GD.Print(child.Name + " has been found");
                btn.Pressed += () => PlaySound(audioStreamPlayers[2]);
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
