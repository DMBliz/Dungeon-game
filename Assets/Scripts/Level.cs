using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Level
{
	public static string startLevel = "fghfhjgfsd";
	public static List<string> levels = new List<string>();
	public static int currentlevel = 0;
	public static bool generated = false;
	public static bool wasHere = false;

	public static void GenerateLevels()
	{
		levels.Add(startLevel);
		generated = true;
		for (int i = 1; i < 10; i++)
		{
			levels.Add(Random.Range(1000, 9999).ToString());
		}
	}

	public static string NextLevel()
	{

		string level = levels[currentlevel];
		return level;
	}

	public static void Clear()
	{
		levels = new List<string>();
		currentlevel = 0;
		generated = false;
		wasHere = false;
	}
}
