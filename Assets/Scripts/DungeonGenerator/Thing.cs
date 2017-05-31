using System;
using UnityEngine;

[Serializable]
public class Thing
{
	public GameObject thingObject;
	[Range(0,100)]
	public float chance;
}
