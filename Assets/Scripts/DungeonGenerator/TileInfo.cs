using UnityEngine;
using System.Collections;

public class TileInfo
{
	private Sprite sprite;
	private bool walkable;
	private bool haveObject;

	public Sprite Sprite
	{
		get { return sprite; }
		set { sprite = value; }
	}

	public bool Walkable
	{
		get { return walkable; }
		set { walkable = value; }
	}

	public bool HaveObject
	{
		get { return haveObject; }
		set { haveObject = value; }
	}

	public TileInfo(Sprite sprite, bool walkable, bool haveObject=false)
	{
		this.sprite = sprite;
		this.walkable = walkable;
		this.haveObject = haveObject;
	}
}
