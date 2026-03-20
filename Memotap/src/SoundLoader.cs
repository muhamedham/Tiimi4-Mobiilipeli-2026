using Godot;
using Godot.Collections;
using System;

public partial class SoundLoader : Node
{
    //TODO jos ääniä lisätään enemmän lisää ne arrayhin josta ne käydään läpi ja alustetaan?
    AudioStreamPlayer correstSoundPlayer = new AudioStreamPlayer();
    [Export] AudioStream correctStream;

    AudioStreamPlayer wrongSoundPlayer = new AudioStreamPlayer();
    [Export] AudioStream wrongStream;

    public void Setup(Array<TileButton> arr)
    {
        correstSoundPlayer.Stream = correctStream;
        //how many sound can be played at once by this player
        correstSoundPlayer.MaxPolyphony = 5;
        AddChild(correstSoundPlayer);

        wrongSoundPlayer.Stream = wrongStream;
        wrongSoundPlayer.MaxPolyphony = 5;
        AddChild(wrongSoundPlayer);

        foreach (TileButton button in arr)
        {
            button.CorrectPress += PlayCorrectSound;
            button.WrongPress += PlayWrongSound;
        }

        Node node = GetParent();

        //CallDeferred(MethodName.NewMethod, node);

    }

/*
//Voisi käyttää jos nappeja ei enää lisätä koodissa vaan käsin
// koodi etsii itse napit ja kuuntelee painallus signaaleja
    private void NewMethod(Node node)
    {
        foreach (Node item in node.GetChildren())
        {
            if (item is TileButton button)
            {
                GD.Print("a tilebutton has been found");
            }

            if (item is Button btn && item is not TileButton)
            {
                GD.Print("a button has been found");
                btn.Pressed += PlayCorrectSound;
            }
            // call recursively it self to find all children
            NewMethod(item);
        }

    }
*/

    private void PlayCorrectSound()
    {
        correstSoundPlayer.Play();
    }

    private void PlayWrongSound()
    {
        wrongSoundPlayer.Play();
    }

    //TODO? Lisää UI nappeihin myös äänet
    // ja silloin kun nappeja näytetään
}

//tutorial:
// https://www.youtube.com/live/QgBecUl_lFs?si=ypeHbF3yeWI40pTe&t=1295
