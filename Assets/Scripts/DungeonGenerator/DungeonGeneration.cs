using UnityEngine;
using System.Collections.Generic;

public class DungeonGeneration : MonoBehaviour
{
	TileInfo[,] Map;

	[SerializeField]
	private int width=50;
	[SerializeField]
	private int height=50;

	[SerializeField]
	private int x=0;
	[SerializeField]
	private int y=0;

	[SerializeField]
	private string seed="seed";

	[SerializeField]
	private GameObject Wall;
	[SerializeField]
	private GameObject Floor;

	[SerializeField]
	private int minLeafSize = 5;
	[SerializeField]
	private int maxLeafSize = 10;

	[SerializeField]
	private RoomStyle[] roomStyles;
	[SerializeField]
	private Sprite[] corridorWalls;
	[SerializeField]
	private Sprite[] corridorFloors;

	[SerializeField]
	GameObject[] enemys;

	private System.Random rnd;

	private void Awake ()
	{
		
		rnd = new System.Random(seed.GetHashCode());
		Map = new TileInfo[width, height];
	}

	public void SetupSettings (Rectangle MapPosSize)
	{
		rnd = new System.Random(seed.GetHashCode());
		width = MapPosSize.width;
		height = MapPosSize.height;
		x = MapPosSize.x;
		y = MapPosSize.y;

		Map = new TileInfo[width, height];
		
	}

	public void SetupSettings (int x, int y, int width, int height)
	{
		rnd = new System.Random(seed.GetHashCode());
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;

		Map = new TileInfo[width, height];
		
	}

	public void Generate ()
	{
		int Max_Leaf_Size = maxLeafSize;

		List<Leaf> Leafs = new List<Leaf>();

		Leaf root = new Leaf(0, 0, width, height, rnd.Next(minLeafSize, maxLeafSize), rnd);
		Leaf l;
		Leafs.Add(root);

		bool didSplit = true;

		while(didSplit)
		{
			didSplit = false;
			for(int i = 0; i < Leafs.Count; i++)
			{
				l = Leafs[i];
				if(l.RightChild == null && l.LeftChild == null)
				{
					if(l.Width > Max_Leaf_Size || l.Height > Max_Leaf_Size)
					{
						if(l.Split())
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

		for(int x = 0; x < width; x++)
		{
			for(int y = 0; y < height; y++)
			{
				Map[x, y] = new TileInfo(corridorWalls[rnd.Next(0, corridorWalls.Length)], false);
			}
		}
		DrawRooms(root);
		DrawHalls(root);
		CheckDist();
	}

	void DrawRooms (Leaf lef)
	{
		if(lef.Room != null)
		{
			RoomStyle style = roomStyles[rnd.Next(roomStyles.Length)];
			for(int x = lef.Room.x; x < lef.Room.right; x++)
			{
				for(int y = lef.Room.y; y < lef.Room.bottom; y++)
				{
					if (x == lef.Room.x || x == lef.Room.right - 1 || y == lef.Room.y || y == lef.Room.bottom - 1)
					{
						
						Map[x, y].Set(style.floors[rnd.Next(style.floors.Length)], false);
					}
					else
					{
						Map[x, y].Set(style.walls[rnd.Next(style.walls.Length)], true);
					}
				}
			}
		}

		if(lef.LeftChild != null)
		{
			DrawRooms(lef.LeftChild);
		}

		if(lef.RightChild != null)
		{
			DrawRooms(lef.RightChild);
		}
	}

	void DrawHalls (Leaf lef)
	{
		if(lef.halls != null)
		{
			List<Rectangle> hls = new List<Rectangle>();
			hls = lef.halls;
			
			for(int i = 0; i < hls.Count; i++)
			{
				Rectangle hl = hls[i];
				for (int x = hl.x; x < hl.right; x++)
				{
					for(int y = hl.y; y < hl.bottom; y++)
					{
						if(!Map[x,y].walkable)
							Map[x, y].Set(corridorFloors[rnd.Next(0, corridorFloors.Length)], true);
					}
				}
			}
		}

		if(lef.LeftChild != null)
		{
			DrawHalls(lef.LeftChild);
		}

		if(lef.RightChild != null)
		{
			DrawHalls(lef.RightChild);
		}
	}

	void CheckDist()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if(!NearestWalkable(new Point(x, y)))
				{
					Map[x, y].sprite = null;
				}
			}
		}
	}

	bool NearestWalkable(Point pos)
	{
		List<TileInfo> infos=new List<TileInfo>();
		for (int i = pos.x - 1; i <= pos.x + 1; i++)
		{
			for (int j = pos.y - 1; j <= pos.y + 1; j++)
			{
				int x = Mathf.Clamp(i, 0, width - 1), y = Mathf.Clamp(j, 0, height - 1);
				infos.Add(Map[x,y]);
			}
		}

		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].walkable)
			{
				return true;
			}
		}

		return false;
	}

	public void SpawnThings(Leaf root)
	{
		if (root.Room != null)
		{

		}

		if (root.LeftChild != null)
		{
			SpawnThings(root.LeftChild);
		}

		if (root.RightChild != null)
		{
			SpawnThings(root.RightChild);
		}
	}

	public void SpawnEnemys(Leaf root)
	{
		if (root.Room != null)
		{

		}

		if (root.LeftChild != null)
		{
			SpawnEnemys(root.LeftChild);
		}

		if (root.RightChild != null)
		{
			SpawnEnemys(root.RightChild);
		}
	}

	public void DrawMap (Vector2 pos)
	{
		GameObject Tile = new GameObject();
		for(int x = 0; x < height; x++)
		{
			for(int y = 0; y < width; y++)
			{
				if (Map[x, y].walkable)
				{
					Tile = Instantiate(Floor, new Vector2((x * 0.1f), this.y + (y / 10f)) + pos, new Quaternion()) as GameObject;
				}
				else
				{
					Tile = Instantiate(Wall, new Vector2((x * 0.1f), this.y + (y / 10f)) + pos, new Quaternion()) as GameObject;
				}
				Tile.GetComponent<SpriteRenderer>().sprite = Map[x, y].sprite;
				Tile.GetComponent<Transform>().parent = gameObject.transform;
			}
		}
	}

	public void DrawMap ()
	{
		GameObject Tile=null;
		for(int i = 0; i < height; i++)
		{
			for(int j = 0; j < width; j++)
			{
				if (Map[i, j].sprite != null)
				{
					if (Map[i, j].walkable)
					{
						Tile = Instantiate(Floor, new Vector2(x - (width - 1) / 2f + i, y - (height - 1) / 2f + j), new Quaternion()) as GameObject;
					}
					else
					{
						Tile = Instantiate(Wall, new Vector2(x - (width - 1) / 2f + i, y - (height - 1) / 2f + j), new Quaternion()) as GameObject;
					}
					Tile.GetComponent<SpriteRenderer>().sprite = Map[i, j].sprite;
					Tile.GetComponent<Transform>().parent = gameObject.transform;
				}
			}
		}
	}
}
