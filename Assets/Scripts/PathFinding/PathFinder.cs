using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
	Grid grid;

	public void Awake()
	{
		grid = GetComponent<Grid>();
	}
	

	public void FindPath(PathRequest request,  Action<PathResult> callback)
	{
		Vector2[] waypoints = new Vector2[0];
		bool pathSucess = false;

		Node startNode = grid.NodeFromWorldPoint(request.pathStart);
		Node targetNode = grid.NodeFromWorldPoint(request.pathEnd);

		if (startNode.walkable && targetNode.walkable)
		{

			Heap<Node> openSet = new Heap<Node>(grid.GridSize);
			HashSet<Node> closedSet = new HashSet<Node>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Node curNode = openSet.RemoveFirst();

				closedSet.Add(curNode);
				if (curNode == targetNode)
				{
					pathSucess = true;
					break;
				}
				List<Node> neighbours = grid.GetNeighbours(curNode);

				foreach (Node neighbour in neighbours)
				{
					if(!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}

					int moveCostToNeighbour = curNode.gCost + getDistance(curNode, neighbour);

					if (moveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = moveCostToNeighbour;
						neighbour.hCost = getDistance(neighbour, targetNode);
						neighbour.parentNode = curNode;

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
						}
						else
						{
							openSet.UpdateItem(neighbour);
						}
					}
				}
			}
		}

		if (pathSucess)
		{
			waypoints = getPath(startNode, targetNode);
			pathSucess = waypoints.Length > 0;
		}
		callback(new PathResult(waypoints, pathSucess, request.callback));
	}

	Vector2[] getPath(Node startNode, Node targetNode)
	{
		List<Node> path = new List<Node>();
		Node curNode = targetNode;

		while (curNode != startNode)
		{
			path.Add(curNode);
			curNode = curNode.parentNode;
		}

		Vector2[] waypoints = SimplifyPath(path);

		Array.Reverse(waypoints);

		return waypoints;
	}

	Vector2[] SimplifyPath(List<Node> path)
	{
		List<Vector2> waypoints = new List<Vector2>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count-1; i++)
		{
			Vector2 directionNew = new Vector2(path[i - 1].gridPosX - path[i].gridPosX, path[i - 1].gridPosY - path[i].gridPosY);
			Vector2 directionNext = new Vector2(path[i].gridPosX - path[i + 1].gridPosX, path[i].gridPosY - path[i + 1].gridPosY);
			if (directionNew != directionOld && directionNext != directionOld)
			{
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}

		return waypoints.ToArray();
	}

	int getDistance(Node a, Node b)
	{
		int distX = Mathf.Abs(a.gridPosX - b.gridPosX);
		int distY = Mathf.Abs(a.gridPosY - b.gridPosY);

		return (distX > distY) ? 14 * distY + 10 * (distX - distY) : 14 * distX + 10 * (distY - distX);
	}
}
