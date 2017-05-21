using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class BaseSpec : ISpecification
{
    public List<BaseSpecModificator> modificators = new List<BaseSpecModificator>();
	public event Action<float> OnValueChange;
	public string name { get { return Name; } set { Name = value; } }
	[SerializeField]
	string Name;
	public string description;
	[SerializeField]
    protected int baseValue;
	public int BaseValue { get { return baseValue; } set { this.baseValue = value; OnValueChange(0f); } }
	[SerializeField]
	protected int value;
	public int Value { get { return value; } set { this.value = value; OnValueChange(0f); } }
	protected int maxValue;
	public int MaxValue { get { return maxValue; } set { this.maxValue = value; } }

	public BaseSpec(string description, int value, int maxValue)
	{
		this.name = this.GetType().ToString();
		this.description = description;
		this.baseValue = value;
		this.maxValue = maxValue;
	}

	public BaseSpec(string name, string description, int value, int maxValue)
	{
		this.name = name;
		this.description = description;
		this.baseValue = value;
		this.maxValue = maxValue;
		CalculateValue();
	}

	public virtual void AddModificator(BaseSpecModificator modificator)
	{
		if (modificator is TimeSpecModificator)
		{
			(modificator as TimeSpecModificator).timeEnd += TimeEnd;
		}else if (modificator is TimeChangeSpecModificator)
		{
			(modificator as TimeChangeSpecModificator).ValueChangeEvent += ValueChange;
			(modificator as TimeChangeSpecModificator).TimeEndEvent += TimeEnd;
		}
		modificators.Add(modificator);
		CalculateValue();
	}

	public virtual void RemoveModificator(BaseSpecModificator modificator)
    {
        if (modificators.Contains(modificator))
        {
            modificators.Remove(modificator);
            CalculateValue();
        }
    }

	protected virtual void TimeEnd(BaseSpecModificator mod)
	{
		if (mod is TimeSpecModificator)
		{
			(mod as TimeSpecModificator).timeEnd -= TimeEnd;
		}if (mod is TimeChangeSpecModificator)
		{
			(mod as TimeChangeSpecModificator).ValueChangeEvent -= ValueChange;
			(mod as TimeChangeSpecModificator).TimeEndEvent -= TimeEnd;
		}
		RemoveModificator(mod);
	}

	protected virtual void ValueChange(float value)
	{
		CalculateValue();
	}

	protected virtual void CalculateValue()
    {
        value = baseValue;
        for (int i = 0; i < modificators.Count; i++)
        {
            if (modificators[i].ModificatorType == EStatModificatorType.Addition)
            {
				value += Mathf.RoundToInt(modificators[i].value);
            }
        }

        for (int i = 0; i < modificators.Count; i++)
        {
            if (modificators[i].ModificatorType == EStatModificatorType.Multiply)
            {
				float calulatedValue = modificators[i].value * value;
				value = Mathf.RoundToInt(calulatedValue);

			}
        }
    }
}
