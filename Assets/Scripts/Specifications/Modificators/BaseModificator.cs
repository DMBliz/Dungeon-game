using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BaseModificator
{
	[SerializeField]
	protected float value;
	[SerializeField]
	protected string name;
	[SerializeField]
	protected string description;
	[SerializeField]
	protected int id;
	[SerializeField]
	protected ModificatorType type;
	[SerializeField]
	protected Sprite icon;

	public float Value
	{
		get { return value; }
	}

	public string Name
	{
		get { return name; }
		set { name = value; }
	}

	public string Description
	{
		get { return description; }
		set { description = value; }
	}

	public int Id
	{
		get { return id; }
		set { id = value; }
	}

	public ModificatorType Type
	{
		get { return type; }
		set { type = value; }
	}

	public Sprite Icon
	{
		get { return icon; }
		set { icon = value; }
	}

	public BaseModificator(string name, string description, ModificatorType type, float value)
	{
		this.name = name;
		this.description = description;
		this.value = value;
		this.type = type;
	}
}

public enum ModificatorType
{
	Additive,
	Multiplaer
}