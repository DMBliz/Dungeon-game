using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class MainSpec : ISpecification
{
	public event Action OnValueZero;
	public List<BaseSpecModificator> modificators = new List<BaseSpecModificator>();

	public string name { get { return Name; } set { Name = value; } }
	[SerializeField]
	string Name;
	public string description;
	public float maxValue;
	protected float value;
	public float Value { get { return value; } set { this.value = value; } }

	public MainSpec(string name, string description, float maxValue) {
		this.name = name;
		this.description = description;
		this.maxValue = maxValue;
		value = maxValue;
	}

	public void AddModificator(BaseSpecModificator modificator)
	{
		if (modificator is TimeSpecModificator)
		{
			ValueChange(modificator.value);
		}
		else if (modificator is TimeChangeSpecModificator)
		{
			(modificator as TimeChangeSpecModificator).ValueChangeEvent += ValueChange;
			(modificator as TimeChangeSpecModificator).TimeEndEvent += TimeEnd;
		}
	}

	public void RemoveModificator(BaseSpecModificator modificator)
	{
		if (modificators.Contains(modificator))
		{
			modificators.Remove(modificator);
		}
	}

	protected void TimeEnd(BaseSpecModificator mod)
	{
		if (mod is TimeChangeSpecModificator)
		{
			(mod as TimeChangeSpecModificator).ValueChangeEvent -= ValueChange;
			(mod as TimeChangeSpecModificator).TimeEndEvent -= TimeEnd;
		}
		RemoveModificator(mod);
	}

	public virtual void ValueChange(float value)
	{
		if (this.value + value <= maxValue)
		{
			this.value += value;
		}
		else
		{
			this.value = maxValue;
		}

		if (this.value < 0)
		{
			this.value = 0;
			if (OnValueZero != null)
				OnValueZero();
		}

	}
}
