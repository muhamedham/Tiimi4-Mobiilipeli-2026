using Godot;

public partial class Translate : Node
{
    [Export] Button English;
    [Export] Button Suomi;
    private string[] languageNames = { "English", "Suomi" };
    private string[] languageCodes = { "en", "fi" };

    public override void _Ready()
    {
        GetNode<Button>("ButtonEnglish").Pressed += OnButtonEnglishPressed;
        GetNode<Button>("ButtonSuomi").ButtonUp += OnButtonSuomiButtonUp;
    }

    private void OnButtonEnglishPressed()
    {
        TranslationServer.SetLocale("en");

    }

    private void OnButtonSuomiButtonUp()
    {
        TranslationServer.SetLocale("fi");
    }
}