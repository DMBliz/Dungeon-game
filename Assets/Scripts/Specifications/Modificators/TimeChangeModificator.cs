using System;
using UnityEngine;
using System.Collections;
using System.Threading;

[Serializable]
public class TimeChangeModificator : TimeModificator
{
	public event Action<TimeChangeModificator> OnValueChange;
	[SerializeField]
	private int coolDownTime;
	private float baseValue;

	public TimeChangeModificator(string name, string description, ModificatorType type, float value, int totalTime, int coolDownTime) : base(name, description, type, value, totalTime)
	{
		this.coolDownTime = coolDownTime;
		baseValue = value;
	}

	protected override void Process()
	{
		while (true)
		{
			if (Time.time > endTime)
				break;

			value += baseValue;

			OnChange();
			Thread.Sleep(coolDownTime);
		}
	}

	void OnChange()
	{
		if (OnValueChange != null)
			OnValueChange(this);
	}
}
