using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldManager : MonoBehaviour
{

	public static WorldManager instance;
	public int NextDungeon;
	public int PreviousDungeon;
	public string Seed;
	public LayerMask unWalkable;
	public bool generateDungeon = false;
	public bool drawFOV = false;
	public DungeonGeneration CurrentDungeon { protected set; get; }

	void Awake()
	{
		instance = this;
		if (generateDungeon)
		{
			CurrentDungeon = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGeneration>();
			if (!Level.generated)
			{
				Level.GenerateLevels();
			}
			else
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpdatePlayer();
			}
			CurrentDungeon.Seed = Level.NextLevel();
			CurrentDungeon.Generate();
			CurrentDungeon.DrawMap();

		}
	}
}
