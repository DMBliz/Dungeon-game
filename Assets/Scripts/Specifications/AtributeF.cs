using System;
using UnityEngine;
using System.Collections;

public class AtributeF : IAtribute
{
	public event Action OnZero;
	private float currentValue;
	private float maxValue;
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

	public float CurrentValue
	{
		get { return currentValue; }
		set
		{
			currentValue = value > maxValue ? maxValue : value;
			if (currentValue <= 0 && OnZero != null)
				OnZero();
		}
	}

	public float MaxValue
	{
		get { return maxValue; }
		set { maxValue = value; }
	}

	public AtributeF(string name, string description, float maxValue)
	{
		this.name = name;
		this.description = description;
		this.maxValue = maxValue;
		currentValue = maxValue;
	}
}
