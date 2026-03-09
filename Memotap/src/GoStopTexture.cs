using Godot;
using System;

//heartTexture inherits Indicator
public partial class GoStopTexture : Indicator
{

//toiminnallisuus tulee Indicator luokasta




    public void SetSize()
    {
        Tween tween = GetTree().CreateTween().SetParallel(true);
        tween.SetEase(Tween.EaseType.In);

        Vector2 koko = new Vector2(400, 400);
        Size = koko;
        Vector2 DefaultPos = Position;
        Vector2 center = GetViewport().GetVisibleRect().Size / 2;
        Position = center;

        tween.TweenProperty(this, "size", CustomMinimumSize,1.0f);
        tween.TweenProperty(this, "position", DefaultPos,1.0f);
    }
}
