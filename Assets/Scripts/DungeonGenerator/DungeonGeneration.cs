using UnityEngine;
using System.Collections.Generic;

public class DungeonGeneration : MonoBehaviour
{
	TileInfo[,] Map;
	[Header("Position settings")]
	[SerializeField]
	private int width = 50;
	[SerializeField]
	private int height = 50;

	[SerializeField]
	private int x = 0;
	[SerializeField]
	private int y = 0;

	public int Width
	{
		get { return width; }
	}

	public int Height
	{
		get { return height; }
	}

	[SerializeField]
	private int minLeafSize = 5;
	[SerializeField]
	private int maxLeafSize = 10;
	private List<Rectangle> rooms = new List<Rectangle>();
	private List<Rectangle> corridors = new List<Rectangle>();

	[Header("Global settings")]
	[SerializeField]
	private bool drawRooms = false;
	[SerializeField]
	private GameObject enter;
	[SerializeField]
	private GameObject exit;
	[SerializeField]
	private GameObject Player;

	[Header("Style settings")]
	[SerializeField]
	private string seed = "seed";

	[SerializeField]
	private GameObject Wall;
	[SerializeField]
	private GameObject Floor;

	[SerializeField]
	private RoomStyle[] roomStyles;
	[SerializeField]
	private Sprite[] corridorWalls;
	[SerializeField]
	private Sprite[] corridorFloors;

	[SerializeField]
	private SpawnableObjects _spawnableObjects;
	[SerializeField]
	private List<GameObject> spawnedThings;

	[Header("Enemys settings")]
	[SerializeField]
	private GameObject[] availableEnemys;

	[SerializeField]
	private List<GameObject> spawnedEnemys;

	[SerializeField]
	private int maxEnemysInRoom = 2;

	[SerializeField]
	private int minPatrolPoints = 3;
	[SerializeField]
	private int maxPatrolPoints = 6;

	[Header("Things Settings")]
	[Range(0,20)]
	private int minSpawnThingsCount = 2;
	[Range(0, 25)]
	private int maxSpawnThingsCount = 5;

	private System.Random rnd;

	private void Awake()
	{
		rnd = new System.Random(seed.GetHashCode());
		Map = new TileInfo[width, height];
	}



	public void Generate()
	{
		int Max_Leaf_Size = maxLeafSize;

		List<Leaf> Leafs = new List<Leaf>();

		Leaf root = new Leaf(0, 0, width, height, rnd.Next(minLeafSize, maxLeafSize), rnd);
		Leaf l;
		Leafs.Add(root);

		bool didSplit = true;

		while (didSplit)
		{
			didSplit = false;
			for (int i = 0; i < Leafs.Count; i++)
			{
				l = Leafs[i];
				if (l.RightChild == null && l.LeftChild == null)
				{
					if (l.Width > Max_Leaf_Size || l.Height > Max_Leaf_Size)
					{
						if (l.Split())
						{
							Leafs.Add(l.LeftChild);
							Leafs.Add(l.RightChild);
							didSplit = true;
						}
					}
				}
			}
		}

		root.CreateRooms();
		RootToArrays(root);
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Map[x, y] = new TileInfo(corridorWalls[rnd.Next(0, corridorWalls.Length)], false);
			}
		}
		DrawRooms();
		DrawHalls();
		CheckDist();
		SpawnThings();
		SpawnEnterExit();
		SpawnEnemys();
	}

	void RootToArrays(Leaf root)
	{
		if (root.Room != null)
		{
			rooms.Add(root.Room);
		}
		if (root.halls != null && root.halls.Count > 0)
		{
			corridors.AddRange(root.halls);
		}

		if (root.LeftChild != null)
		{
			RootToArrays(root.LeftChild);
		}

		if (root.RightChild != null)
		{
			RootToArrays(root.RightChild);
		}
	}

	void DrawRooms()
	{
		foreach (Rectangle room in rooms)
		{
			RoomStyle style = roomStyles[rnd.Next(roomStyles.Length)];
			for (int x = room.x; x < room.right; x++)
			{
				for (int y = room.y; y < room.bottom; y++)
				{
					if (x == room.x || x == room.right - 1 || y == room.y || y == room.bottom - 1)
					{
						Map[x, y].Sprite = style.walls[rnd.Next(style.walls.Length)];
					}
					else
					{
						Map[x, y].Sprite = style.floors[rnd.Next(style.floors.Length)];
						Map[x, y].Walkable = true;
					}
				}
			}
		}
	}

	void DrawHalls()
	{
		foreach (Rectangle hall in corridors)
		{
			for (int x = hall.x; x < hall.right; x++)
			{
				for (int y = hall.y; y < hall.bottom; y++)
				{
					if (!Map[x, y].Walkable)
					{
						Map[x, y].Sprite = corridorFloors[rnd.Next(0, corridorFloors.Length)];
						Map[x, y].Walkable = true;
					}
				}
			}
		}
	}

	void CheckDist()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (!NearestWalkable(new Point(x, y)))
				{
					Map[x, y].Sprite = null;
				}
			}
		}
	}

	bool NearestWalkable(Point pos)
	{
		List<TileInfo> infos = new List<TileInfo>();
		for (int i = pos.x - 1; i <= pos.x + 1; i++)
		{
			for (int j = pos.y - 1; j <= pos.y + 1; j++)
			{
				int x = Mathf.Clamp(i, 0, width - 1), y = Mathf.Clamp(j, 0, height - 1);
				infos.Add(Map[x, y]);
			}
		}

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].Walkable)
			{
				return true;
			}
		}

		return false;
	}

	void SpawnThings()
	{
		foreach (Rectangle room in rooms)
		{
			for (int x = room.left + 1; x < room.right - 1; x++)
			{
				for (int y = room.top + 1; y < room.bottom - 1; y++)
				{
					float chance = (float)rnd.NextDouble() * 100;
					List<Thing> canSpawn = _spawnableObjects.ThingsByPercent(chance);
					if (canSpawn.Count > 0)
					{
						Thing spawnThing = canSpawn[rnd.Next(0, canSpawn.Count - 1)];
						Map[x, y].HaveObject = true;
						GameObject spawnedThing = Instantiate(spawnThing.thingObject, ToWorldPosition(new Vector2(x, y)), new Quaternion());
						spawnedThings.Add(spawnedThing);
						spawnedThing.transform.parent = transform.GetChild(1);
						Inventory temp = spawnedThing.GetComponent<Inventory>();
						if (temp != null)
						{
							AddItemsToInventory(temp);
						}
					}
				}
			}
		}
	}

	void AddItemsToInventory(Inventory inventory)
	{
		int itemsCount = rnd.Next(minSpawnThingsCount, maxSpawnThingsCount);

		for (int i = 0; i < itemsCount; i++)
		{
			Thing item = _spawnableObjects.GetRandomThing();
			GameObject temp = Instantiate(item.thingObject, ToWorldPosition(new Vector2(inventory.transform.position.x, inventory.transform.position.y)), new Quaternion());
			inventory.AddItem(temp.GetComponent<Item>());
		}
	}

	void SpawnEnemys()
	{
		foreach (Rectangle room in rooms)
		{
			int enemyCount = 0;
			for (int x = room.left + 1; x < room.right - 1; x++)
			{
				for (int y = room.top + 1; y < room.bottom - 1; y++)
				{
					float chance = (float)rnd.NextDouble() * 100;
					if (chance >= 98 && enemyCount<maxEnemysInRoom)
					{
						Map[x, y].HaveObject = true;
						GameObject enemy = Instantiate(availableEnemys[rnd.Next(0, availableEnemys.Length)], ToWorldPosition(new Vector2(x, y)), new Quaternion());
						spawnedEnemys.Add(enemy);
						enemyCount++;
						MakePatrolPoints(enemy, room);
						enemy.transform.parent = transform.GetChild(2);
						AddItemsToInventory(enemy.GetComponent<Inventory>());
					}
				}
			}
		}
	}

	void MakePatrolPoints(GameObject enemy, Rectangle room)
	{
		BasicEnemy enemyC = enemy.GetComponent<BasicEnemy>();
		enemyC.Fsm.patrolPoints.Clear();
		enemyC.Fsm.patrolPoints.Add(enemyC.transform.position);
		int patroolPointsCount = rnd.Next(minPatrolPoints, maxPatrolPoints);

		for (int i = 1; i < patroolPointsCount; i++)
		{
			int x = 0;
			int y = 0;
			do
			{
				x = rnd.Next(room.left + 1, room.right - 1);
				y = rnd.Next(room.top + 1, room.bottom - 1);
			} while (Map[x, y].HaveObject);

			enemyC.Fsm.patrolPoints.Add(ToWorldPosition(new Vector2(x, y)));
		}
	}

	void SpawnEnterExit()
	{
		Rectangle startRoom = rooms[0];
		Rectangle endRoom = rooms[rooms.Count - 1];

		Point spawnPos = new Point(startRoom.left + 1 + startRoom.width / 2 + rnd.Next(0, startRoom.width / 2), startRoom.top + 1 + startRoom.height / 2 + rnd.Next(0, startRoom.height / 2));
		Point exitPos = new Point(endRoom.left + 1 + endRoom.width / 2 + rnd.Next(0, endRoom.width / 2), endRoom.top + 1 + endRoom.height / 2 + rnd.Next(0, endRoom.height / 2));

		int count = 0;
		while (Map[spawnPos.x, spawnPos.y].HaveObject || !Map[spawnPos.x, spawnPos.y].Walkable)
		{
			spawnPos = new Point(startRoom.left + 1 + startRoom.width / 2 + rnd.Next(0, startRoom.width / 2), startRoom.top + 1 + startRoom.height / 2 + rnd.Next(0, startRoom.height / 2));
			count++;
			if (count > 500)
			{
				spawnPos = new Point(rnd.Next(startRoom.x+1, startRoom.right-1), rnd.Next(startRoom.y+1, startRoom.bottom-1));
			}

			if (count > 1000)
			{
				return;
			}
		}

		count = 0;

		while (Map[exitPos.x, exitPos.y].HaveObject || !Map[exitPos.x, exitPos.y].Walkable)
		{
			exitPos = new Point(endRoom.left + 1 + endRoom.width / 2 + rnd.Next(0, endRoom.width / 2), endRoom.top + 1 + endRoom.height / 2 + rnd.Next(0, endRoom.height / 2));
			count++;

			if (count > 500)
			{
				exitPos = new Point(rnd.Next(endRoom.left, endRoom.right-1),rnd.Next(endRoom.top, endRoom.bottom-1));
			}

			if (count > 1000)
			{
				return;
			}
		}

		Instantiate(enter, ToWorldPosition(new Vector2(spawnPos.x, spawnPos.y)), new Quaternion());
		Instantiate(exit, ToWorldPosition(new Vector2(exitPos.x, exitPos.y)), new Quaternion());
		Instantiate(Player, ToWorldPosition(new Vector2(spawnPos.x, spawnPos.y)), new Quaternion());
	}

	public void DrawMap()
	{
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				if (Map[i, j].Sprite != null)
				{
					GameObject tile;
					if (Map[i, j].Walkable)
					{
						tile = Instantiate(Floor, new Vector2(x - (width - 1) / 2f + i, y - (height - 1) / 2f + j), new Quaternion()) as GameObject;
					}
					else
					{
						tile = Instantiate(Wall, new Vector2(x - (width - 1) / 2f + i, y - (height - 1) / 2f + j), new Quaternion()) as GameObject;
					}
					tile.GetComponent<SpriteRenderer>().sprite = Map[i, j].Sprite;
					tile.GetComponent<Transform>().parent = transform.GetChild(0);
				}
			}
		}
	}

	void OnDrawGizmos()
	{
		if (drawRooms)
		{
			DrawRoomsGizmos();
		}
	}

	void DrawRoomsGizmos()
	{
		if (rooms != null && rooms.Count > 0)
		{
			Gizmos.color = Color.blue;
			foreach (Rectangle room in rooms)
			{
				Gizmos.DrawWireCube(RectangleToWorld(room), new Vector3(room.width, room.height, 1));
			}
		}
		if (corridors != null && corridors.Count > 0)
		{
			Gizmos.color = Color.white;
			foreach (Rectangle corridor in corridors)
			{
				Gizmos.DrawWireCube(RectangleToWorld(corridor), new Vector3(corridor.width, corridor.height, 1));
			}
		}
	}

	Vector2 RectangleToWorld(Rectangle rect)
	{
		return new Vector2(x + rect.left + rect.width / 2f - width / 2f, y + rect.top + rect.height / 2f - height / 2f);
	}

	Vector2 ToWorldPosition(Vector2 pos)
	{
		return new Vector2(x + pos.x + 0.5f - width / 2f, y + pos.y + 0.5f - height / 2f);
	}
}