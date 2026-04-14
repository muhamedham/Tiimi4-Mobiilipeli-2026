using Godot;
public partial class Translate : Node
{
    [Export] Button English;
    [Export] Button Suomi;
    [Export] Button Svenska;

    private string[] languageNames = { "English", "Suomi", "Svenska" };
    private string[] languageCodes = { "en", "fi", "sv" };

    public override void _Ready()
    {
        English.Pressed += OnButtonEnglishPressed;
        Suomi.Pressed += OnButtonSuomiPressed;
        Svenska.Pressed += OnButtonSvenskaPressed;

        English.Visible = false;
        Suomi.Visible = true;
        Svenska.Visible = false;
    }

    private void OnButtonEnglishPressed()
    {
        TranslationServer.SetLocale("en");
        English.Visible = false;
        Suomi.Visible = true;
        Svenska.Visible = false;
    }

    private void OnButtonSuomiPressed()
    {
        TranslationServer.SetLocale("fi");
        English.Visible = false;
        Suomi.Visible = false;
        Svenska.Visible = true;
    }

    private void OnButtonSvenskaPressed()
    {
        TranslationServer.SetLocale("sv");
        English.Visible = true;
        Suomi.Visible = false;
        Svenska.Visible = false;
    }
}