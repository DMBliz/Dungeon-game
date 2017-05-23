using UnityEngine;
using System;

public class Atribute : IAtribute
{
	public event Action OnZero;
	private int minValue;
	private int currentValue;
	private int maxValue;
	private string name;
	private string description;

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

	public int CurrentValue
	{
		get { return currentValue; }
		set
		{
			currentValue = value > maxValue ? maxValue : value;
		}
	}

	public int MaxValue
	{
		get { return maxValue; }
		set { maxValue = value; }
	}

	public Atribute(string name, string description, int minValue, int maxValue)
	{
		this.name = name;
		this.description = description;
		this.minValue = minValue;
		this.maxValue = maxValue;
		currentValue = minValue;
	}


}
