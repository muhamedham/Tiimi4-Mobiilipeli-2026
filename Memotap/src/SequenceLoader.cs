using Godot;
using System;

using System.Collections.Generic;
using Godot.Collections;
using System.Linq;

/*
    This node is responsible for the loading of levels described under ./res/levels/
    by reading them and placing into nested arrays, giving the Game-node usable
    2D-arrays of sequences.
*/
public partial class SequenceLoader : Node
{
    // The directory where all '.txt' level files reside
    private const string _seqDir = "./res/levels";

    public static int[][] LoadSequences(string level)
        {

        if (!FileAccess.FileExists(level))
        {
            GD.PrintErr($"SequenceLoader: file not found at {_seqDir + level}");
            return System.Array.Empty<int[]>();
        }

        using var file = FileAccess.Open(_seqDir + level, FileAccess.ModeFlags.Read);
        var sequences = new List<int[]>();

        while (!file.EofReached())
        {
            string line = file.GetLine().Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.All(p => int.TryParse(p, out _)))
                sequences.Add(System.Array.ConvertAll(parts, int.Parse));
            else
                GD.PrintErr($"SequenceLoader: skipping malformed line \"{line}\" in {level}.txt");
        }

        return sequences.ToArray();
    }
    public static string[] GetAvailableLevels()
    {
        var dir = DirAccess.Open(_seqDir);

        if (dir == null)
        {
            GD.Print("could not find level directory");
            return [];
        }

        Array <string> levels = new();
        dir.ListDirBegin();

        string fileName = dir.GetNext();

        while (fileName != String.Empty)
        {
            if (!dir.CurrentIsDir() && fileName.EndsWith(".txt"))
            {
                levels.Add(fileName);
            }
            fileName = dir.GetNext();
        }

        dir.ListDirEnd();
        return levels.ToArray();
    }
}