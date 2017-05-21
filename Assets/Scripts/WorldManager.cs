using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldManager : MonoBehaviour
{

	public static WorldManager worldManager;
	public static float minDistToMove = 0.05f;
	public int NextDungeon;
	public int PreviousDungeon;
	public string Seed;
	public LayerMask unWalkable;
	public bool generateDungeon = false;
	public DungeonGeneration CurrentDungeon { protected set; get; }

	void Awake()
	{
		worldManager = this;
		if (generateDungeon)
		{
			CurrentDungeon = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonGeneration>();
			CurrentDungeon.Generate();
			CurrentDungeon.DrawMap();
		}
	}

	void Update()
	{

	}
	
}
