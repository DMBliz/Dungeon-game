using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
	public LayerMask unWalkable;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	private Node[,] map;
	public bool drawGizmos = false;
	public float minDistFromObstacle = 1f;

	public Node[,] Nodes { get { return map; } }

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	public int GridSize { get { return gridSizeX * gridSizeY; } }

	public void Start()
	{
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}

	public Node NodeFromWorldPoint(Vector2 point)
	{
		float percentx = Mathf.Clamp01((point.x + gridWorldSize.x / 2f) / gridWorldSize.x);
		float percenty = Mathf.Clamp01((point.y + gridWorldSize.y / 2f) / gridWorldSize.y);

		int x = Mathf.RoundToInt((gridSizeX - 1) * percentx);
		int y = Mathf.RoundToInt((gridSizeY - 1) * percenty);
		return map[x, y];
	}

	public void CreateGrid()
	{
		map = new Node[gridSizeX, gridSizeY];
		Vector2 bottomLeft = new Vector2(transform.position.x, transform.position.y) - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;
		for (int i = 0; i < gridSizeX; i++)
		{
			for (int j = 0; j < gridSizeY; j++)
			{
				Vector2 worldPoint = bottomLeft + Vector2.right * (i * nodeDiameter + nodeRadius) + Vector2.up * (j * nodeDiameter + nodeRadius);
				bool walkable = Physics2D.OverlapBox(worldPoint, Vector2.one * nodeRadius, 0, unWalkable) == null;

				map[i, j] = new Node(worldPoint, i, j, walkable, !walkable);
			}
		}

		for (int i = 0; i < gridSizeX; i++)
		{
			for (int j = 0; j < gridSizeY; j++)
			{
				if (map[i, j].walkable)
				{
					if (CheckDistance(map[i, j]))
					{
						map[i, j].walkable = false;
						map[i, j].haveCollider = false;
					}
				}
			}
		}
	}

	bool CheckDistance(Node node)
	{
		List<Node> checkNodes = GetNeighboursInDistance(node, minDistFromObstacle);

		foreach (var item in checkNodes)
		{
			if (item.haveCollider)
			{
				return true;
			}
		}
		return false;
	}

	List<Node> GetNeighboursInDistance(Node startNode,float distance)
	{
		List<Node> neighbours = new List<Node>();

		int numb = Mathf.RoundToInt(distance / nodeRadius);
		int startX = Mathf.Clamp(startNode.gridPosX - numb, 0, gridSizeX - 1);
		int endX = Mathf.Clamp(startNode.gridPosX + numb, 0, gridSizeX - 1);

		int startY = Mathf.Clamp(startNode.gridPosY - numb, 0, gridSizeY - 1);
		int endY = Mathf.Clamp(startNode.gridPosY + numb, 0, gridSizeY - 1);

		for (int x = startX; x <= endX; x++)
		{
			for (int y = startY; y <= endY; y++)
			{
				neighbours.Add(map[x, y]);
			}
		}

		return neighbours;
	}
	
	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if(i==0 && j == 0)
				{
					continue;
				}
				int checkX = node.gridPosX + i;
				int checkY = node.gridPosY + j;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) 
				{
					neighbours.Add(map[checkX, checkY]);
				}
			}
		}
		return neighbours;
	}

	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));
		if (map != null && drawGizmos)
		{
			foreach (var item in map)
			{
				Gizmos.color = !item.haveCollider ? Color.white : Color.red;
				Gizmos.color = !item.haveCollider && !item.walkable ? Color.blue : Gizmos.color;
				Gizmos.DrawCube(item.worldPosition, Vector3.one * nodeDiameter);
			}
		}
	}
}
