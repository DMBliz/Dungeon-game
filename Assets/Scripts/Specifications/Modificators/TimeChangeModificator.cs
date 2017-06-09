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

	public TimeChangeModificator(string name, string description, ModificatorType type, float value, int totalTime, int coolDownTime) : base(name, description, type, value, totalTime)
	{
		this.coolDownTime = coolDownTime;
	}

	protected override IEnumerator Process()
	{
		while (Time.time < endTime)
		{
			yield return new WaitForSeconds(coolDownTime);
			OnChange();
		}

		OnEnd();
	}

	void OnChange()
	{
		if (OnValueChange != null)
			OnValueChange(this);

	}
}
