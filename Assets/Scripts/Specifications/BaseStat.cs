using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat
{
	public event Action<BaseModificator> OnAddModificator;
	public event Action<BaseModificator> OnRemoveModificator;

	[SerializeField]
	private List<BaseModificator> modificators = new List<BaseModificator>();
	private int baseValue;
	[SerializeField]
	private int finalValue;
	[SerializeField]
	private string statName;
	private string statDescription;

	public int BaseValue
	{
		get { return baseValue; }
	}

	public string StatName
	{
		get { return statName; }
		set { statName = value; }
	}

	public string StatDescription
	{
		get { return statDescription; }
		set { statDescription = value; }
	}

	public int FinalValue
	{
		get { return finalValue; }
	}

	public BaseStat(string statName, string statDescription, int baseValue)
	{
		this.baseValue = baseValue;
		this.statName = statName;
		this.statDescription = statDescription;
		finalValue = baseValue;
	}

	public void SetLevel(int level)
	{
		baseValue = level;
		CalculateFinalValue();
	}

	public void IncreaseLevel(int level)
	{
		baseValue += level;
		CalculateFinalValue();
	}

	public void AddModificator(BaseModificator modificator)
	{
		if (modificator is TimeModificator)
		{
			(modificator as TimeModificator).OnTimeEnd += OnEnd;
			modificators.Add(modificator);
			CalculateFinalValue();

			if (OnAddModificator != null)
				OnAddModificator(modificator);
		}
	}

	public void RemoveModificator(BaseModificator modificator)
	{
		modificators.Remove(modificator);
		CalculateFinalValue();

		if (OnRemoveModificator != null)
			OnRemoveModificator(modificator);
	}

	protected virtual void OnEnd(BaseModificator modificator)
	{
		RemoveModificator(modificator);
	}

	void CalculateFinalValue()
	{
		finalValue = baseValue;
		foreach (BaseModificator modificator in modificators)
		{
			if (modificator.Type == ModificatorType.Additive)
			{
				finalValue += (int)modificator.Value;
			}
		}

		foreach (BaseModificator modificator in modificators)
		{
			if (modificator.Type == ModificatorType.Multiplaer)
			{
				finalValue *= (int)modificator.Value;
			}
		}
	}
}
