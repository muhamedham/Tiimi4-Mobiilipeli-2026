using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;

public partial class Score : TextureRect
{
	// Fixed path where level textures reside
	[ExportGroup("Level Texture path")]
	[Export] private string _lvlPath = "./art/level";

	// Array of loaded textures
	private List<Texture2D> _textures = new List<Texture2D>();

	// Called when node enters tree
    public override void _Ready()
    {
        FetchTextures();
    }

	// fetch level textures from ./art/level
	private void FetchTextures()
	{
		// Assign dir access to a variable and open stream
		var dirStream = DirAccess.Open("./art/level");

		// Check validity of stream
		if (dirStream == null)
		{
			GD.Print($"couldn't find {_lvlPath}");
			return;
		}

		var fileNames = new List<string>();

		// Start reading
		dirStream.ListDirBegin();

		// Assing next value in dir to string
		string fileName = dirStream.GetNext();

		while (fileName != String.Empty)
		{
			// if filename ends within png in this dir, its a level
			if (fileName.EndsWith(".png"))
			{
				fileNames.Add(fileName);
			}
			
			fileName = dirStream.GetNext();
		}

		// Stop reading
		dirStream.ListDirEnd();

		// Sorting logic to assure all levels are in correct order
		fileNames.Sort((a, b) =>
		{
			int numA = ExtractNumber(a);
			int numB = ExtractNumber(b);
			return numA.CompareTo(numB);
		});

		// Finally load textures into array
		foreach (string line in fileNames)
		{
			_textures.Add(GD.Load<Texture2D>($"{_lvlPath}/{line}"));
		}
	}

	// Method for setting the current level, called in Game.cs
	public void SetLevel(int levelIndex)
	{
		Texture = _textures[levelIndex];
	}

	private int ExtractNumber(string line)
	{
		string digits = new string(line.Where(char.IsDigit).ToArray());
		return int.TryParse(digits, out int n) ? n : -1;
	}
}

