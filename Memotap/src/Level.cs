using System;
using Godot;
using Godot.Collections;


/*
[
    [0, 1, 2]
    [3, 4, 5]
    [6, 7, 8]
]
*/


public static class File
{

    public static Array<Array<Array<int>>> LoadFile()
    {
        String filePath = "res://Leveljs.json";

        if(!FileAccess.FileExists(filePath))
        {
            GD.PushError($"File not found in {filePath}");
        }

        using var saveFile = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);

        var json = new Json();
        var parseResult = json.Parse(saveFile.GetAsText());
        if (parseResult != Error.Ok)
        {
            GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in level file at line {json.GetErrorLine()}");
        }

        Dictionary dict = json.Data.AsGodotDictionary();

        Array<Array<Array<int>>> Levels = new();


        foreach (var (key, value) in dict)
        {
            var variations = value.AsGodotArray();
            var levelVariations = new Array<Array<int>>();


            foreach (var variation in variations)
            {
                levelVariations.Add(variation.AsGodotArray<int>());
            }

            Levels.Add(levelVariations);
        }
        return Levels;
    }
}

