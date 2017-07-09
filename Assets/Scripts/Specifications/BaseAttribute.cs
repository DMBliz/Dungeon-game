using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseAttribute
{
	public event Action<BaseModificator> OnAddModificator;
	public event Action<BaseModificator> OnRemoveModificator;
	public event Action OnZero;
	public event Action<BaseAttribute,float> OnValueChange;
	[SerializeField]
	private List<BaseModificator> modificators = new List<BaseModificator>();
	[SerializeField]
	private float maxValue;
	[SerializeField]
	private float finalValue;
	[SerializeField]
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
		set
		{
			float temp = finalValue;
			finalValue = value;
			if (finalValue > maxValue) finalValue = maxValue;
			if (OnValueChange != null)
				OnValueChange(this, finalValue - temp);
			if (finalValue <= 0 && OnZero != null)
			{
				OnZero();
				finalValue = 0;
			}
		}
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
			(modificator as TimeModificator).Start();
		}
		if (modificator is TimeChangeModificator)
		{
			(modificator as TimeChangeModificator).OnValueChange += ValueChange;
		}

		if (modificator.Type == ModificatorType.Multiplaer)
		{
			TimeModificator temp = (TimeModificator)modificator;
			if (temp != null && temp.IsMax)
			{
				maxValue *= modificator.Value;
			}
			FinalValue *= modificator.Value;
		}

		if (modificator.GetType() != typeof(BaseModificator))
		{
			modificators.Add(modificator);
			if (OnAddModificator != null)
				OnAddModificator(modificator);
		}
		else
		{
			if (modificator.Type == ModificatorType.Additive)
			{
				FinalValue += modificator.Value;
			}
		}

	}

	public void RemoveModificator(BaseModificator modificator)
	{
		modificators.Remove(modificator);

		if (modificator.Type == ModificatorType.Multiplaer)
		{
			FinalValue /= modificator.Value;
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
			(modificator as TimeChangeModificator).OnValueChange -= ValueChange;
		}

		if (OnRemoveModificator != null)
			OnRemoveModificator(modificator);
	}

	protected virtual void OnEnd(BaseModificator modificator)
	{
		RemoveModificator(modificator);
	}

	protected virtual void ValueChange(TimeChangeModificator modificator)
	{
		FinalValue += modificator.Value;

		if (OnValueChange != null)
			OnValueChange(this,modificator.Value);
	}
}
