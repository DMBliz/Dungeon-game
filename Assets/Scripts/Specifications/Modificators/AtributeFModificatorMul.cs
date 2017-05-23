using System;
using UnityEngine;
using System.Collections;
using System.Threading;

public class AtributeFModificatorMul : IModificator
{
	public event Action<IModificator> OnEnd;
	private string name;
	private string description;
	private float time;
	private float value;
	private AtributeF atr;

	public Action a { get; set; }
	public string Name { get { return name; } set { name = value; } }
	public string Description { get { return description; } set { description = value; } }
	public float WorkTime { get { return time; } set { time = value; } }

	public AtributeFModificatorMul(string name, string description, float value, float time, AtributeF atr)
	{
		this.name = name;
		this.description = description;
		this.value = value;
		this.time = time;
		this.atr = atr;
	}

	public void Start()
	{
		a = new Action(Work);
		a.BeginInvoke(null, null);
	}

	void Work()
	{
		float endTime = time + Time.time;

		atr.CurrentValue *= value;
		atr.MaxValue *= value;
		Thread.Sleep(TimeSpan.FromSeconds(endTime));

		if (OnEnd != null)
		{
			OnEnd(this);
			a.EndInvoke(null);
		}
		atr.MaxValue /= value;
		atr.CurrentValue = atr.CurrentValue;
	}
}
