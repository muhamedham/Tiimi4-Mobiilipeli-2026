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
    private const string _lvlDir = "res://levels";

    // This function receives a <string> parameter representing the filename of the level
    // corresponding the current level. Based on that, a 2D array of sequences is returned
    // from the file.
    public static int[][] LoadSequences(string level)
    {
        // Check if a file is found and handle unfound ones
        if (!FileAccess.FileExists( _lvlDir + "/" + level))
        {
            GD.PushError($"SequenceLoader: file not found at {_lvlDir + "/" + level}");
            return [];
        }

        // Variables for the file and the upcoming sequences
        using var file = FileAccess.Open(_lvlDir + "/" + level, FileAccess.ModeFlags.Read);
        var sequences = new List<int[]>();

        // While the EOF hasnt been reached, iterate through lines collecting
        // sequences and appending them to the 2D array
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

    // This function returns the names of all files, and should be used so that
    // the game doesn't end up iterating out of bounds.
    public static Array<string> GetAvailableLevels()
    {

        // Assign and chek if a directory with given name is found
        var dir = DirAccess.Open(_lvlDir);

        if (dir == null)
        {
            GD.Print($"could not find level directory {_lvlDir}");
            return [];
        }

        // Create the name array and initialize the 'DirAccess' streams
        Array <string> levels = new();
        dir.ListDirBegin();


        // Iterate through filenames ending in '.txt' until none are found
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

        // Needed because during testing array has been scrambled.
        levels.Sort();
        return levels;
    }
}