using UnityEngine;
using System.Collections;

public class TileInfo
{
	public Sprite sprite;
	public bool walkable;

	public TileInfo(Sprite sprite,bool walkable)
	{
		this.sprite = sprite;
		this.walkable = walkable;
	}

	public void Set(Sprite sprite, bool walkable)
	{
		this.sprite = sprite;
		this.walkable = walkable;
	}
}
