using UnityEngine;
using System.Collections;
using System;

public class Node : IHeapItem<Node>
{
	public bool walkable;
	public bool haveCollider;
	public Node parentNode;
	public Vector2 worldPosition;
	public int gridPosX;
	public int gridPosY;

	public int gCost;
	public int hCost;

	int heapIndex;

	public int fCost { get { return gCost + hCost; } }

	public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }

	public Node(Vector2 position, int gridPosX, int gridPosY, bool walkAble, bool haveCollider)
	{
		worldPosition = position;
		this.walkable = walkAble;
		this.gridPosX = gridPosX;
		this.gridPosY = gridPosY;
		this.haveCollider = haveCollider;
	}

	public int CompareTo(Node compareItem)
	{
		int compare = fCost.CompareTo(compareItem.fCost);
		if (compare == 0)
		{
			compare = hCost.CompareTo(compareItem.hCost);
		}

		return -compare;
	}
}
