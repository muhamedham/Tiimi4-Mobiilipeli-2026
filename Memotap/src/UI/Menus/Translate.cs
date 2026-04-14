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

        string locale = TranslationServer.GetLocale();
        if (locale != "en" && locale != "fi" && locale != "sv")
        locale = "en";
        English.Visible = locale == "en";
        Suomi.Visible = locale == "fi";
        Svenska.Visible = locale == "sv";
    }

    private void OnButtonEnglishPressed()
    {
        TranslationServer.SetLocale("fi");
        English.Visible = false;
        Suomi.Visible = true;
        Svenska.Visible = false;
    }

    private void OnButtonSuomiPressed()
    {
        TranslationServer.SetLocale("sv");
        English.Visible = false;
        Suomi.Visible = false;
        Svenska.Visible = true;
    }

    private void OnButtonSvenskaPressed()
    {
        TranslationServer.SetLocale("en");
        English.Visible = true;
        Suomi.Visible = false;
        Svenska.Visible = false;
    }
}