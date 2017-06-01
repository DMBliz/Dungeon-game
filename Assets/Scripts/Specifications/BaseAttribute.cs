using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseAttribute
{
	public event Action<BaseModificator> OnAddModificator;
	public event Action<BaseModificator> OnRemoveModificator;

	private List<BaseModificator> modificators = new List<BaseModificator>();
	private float maxValue;
	private float finalValue;
	private string name;
	private string description;

	public float MaxValue
	{
		get { return maxValue; }
		set { maxValue = value; }
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

	public float FinalValue
	{
		get { return finalValue; }
		set { finalValue = value; }
	}

	public BaseAttribute(string name, string description, float maxValue)
	{
		this.maxValue = maxValue;
		this.name = name;
		this.description = description;
		finalValue = maxValue;
	}

	public void AddModificator(BaseModificator modificator)
	{
		if (modificator is TimeModificator)
		{
			(modificator as TimeModificator).OnTimeEnd += OnEnd;
		}
		if (modificator is TimeChangeModificator)
		{
			(modificator as TimeChangeModificator).OnValueChange += OnValueChange;
		}
		
		modificators.Add(modificator);

		if (modificator.Type == ModificatorType.Multiplaer)
		{
			finalValue *= modificator.Value;
			TimeModificator temp = (TimeModificator) modificator;
			if (temp != null && temp.IsMax)
			{
				maxValue *= modificator.Value;
			}
		}

		if (finalValue > maxValue)
			finalValue = maxValue;

		if (OnAddModificator != null)
			OnAddModificator(modificator);
	}

	public void RemoveModificator(BaseModificator modificator)
	{
		modificators.Remove(modificator);

		if (modificator.Type == ModificatorType.Multiplaer)
		{
			finalValue /= modificator.Value;
			TimeModificator temp = (TimeModificator)modificator;
			if (temp != null && temp.IsMax)
			{
				maxValue /= modificator.Value;
			}
		}

		if (modificator is TimeModificator)
		{
			(modificator as TimeModificator).OnTimeEnd -= OnEnd;
		}
		if (modificator is TimeChangeModificator)
		{
			(modificator as TimeChangeModificator).OnValueChange -= OnValueChange;
		}

		if (finalValue > maxValue)
			finalValue = maxValue;

		if (OnRemoveModificator != null)
			OnRemoveModificator(modificator);
	}

	protected virtual void OnEnd(BaseModificator modificator)
	{
		RemoveModificator(modificator);
	}

	protected virtual void OnValueChange(TimeChangeModificator modificator)
	{
		finalValue += modificator.Value;
		if (finalValue > maxValue)
			finalValue = maxValue;
	}
}
