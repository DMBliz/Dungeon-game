using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ObjectGroup
{
	[SerializeField]
	public List<Thing> spawnableObjects;
	[Range(0, 100)]
	public float GroupChance = 0f;

}
