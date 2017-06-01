using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class TestEnemy
{
	[SerializeField]
	private Rectangle pos;
	[SerializeField]
	private bool showPoints;
	[SerializeField]
	private List<Vector2> patrolPoints = new List<Vector2>();

	public Rectangle Pos
	{
		get { return pos; }
		set { pos = value; }
	}

	public bool ShowPoints
	{
		get { return showPoints; }
		set { showPoints = value; }
	}

	public List<Vector2> PatrolPoints
	{
		get { return patrolPoints; }
		set { patrolPoints = value; }
	}

	public TestEnemy(Rectangle pos)
	{
		this.pos = pos;
	}
}
