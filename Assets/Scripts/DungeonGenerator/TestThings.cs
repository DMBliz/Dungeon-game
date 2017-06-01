using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TestThings
{
	public Rectangle pos;
	public EThingType thingType;

	public TestThings(Rectangle pos, EThingType thingType)
	{
		this.pos = pos;
		this.thingType = thingType;
	}
}

public enum EThingType
{
	Container,
	Thing,
	Furniture
}