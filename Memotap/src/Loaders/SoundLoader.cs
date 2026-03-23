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
                Stream = audioStreams[i],

                Bus = "tausta"
            };


            AddChild(audioStreamPlayers[i]);
        }

        //audioStreamPlayers[2].Bus = "tausta";

        foreach (TileButton button in arr)
        {
            button.CorrectPress += () => PlaySound(audioStreamPlayers[0]);
            button.WrongPress += () => PlaySound(audioStreamPlayers[1]);
        }

        //parent is Game
        Node Parent = GetParent();
        CallDeferred(MethodName.FindChildren, Parent);

    }
    private void PlaySound(AudioStreamPlayer player)
    {
        player.Play();
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
