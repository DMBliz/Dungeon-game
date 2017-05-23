using System;
using UnityEngine;
using System.Collections;
using System.Threading;

public class AtributeFModificatorAdd : IModificator
{
	public event Action<IModificator> OnEnd;
	public Action a { get; set; }
	private string name;
	private string description;
	private float value;
	private float time;
	private float changeTime;
	private AtributeF atr;

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

	public float WorkTime
	{
		get { return time; }
		set { time = value; }
	}

	public AtributeFModificatorAdd(string name, string description, float value, float changeTime, float fullTime,AtributeF atr)
	{
		this.name = name;
		this.description = description;
		this.value = value;
		this.time = fullTime;
		this.changeTime = changeTime;
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
		while (endTime > Time.time)
		{
			Thread.Sleep(TimeSpan.FromSeconds(changeTime));
			atr.CurrentValue += value;
		}
		if (OnEnd != null)
		{
			OnEnd(this);
			a.EndInvoke(null);
		}
	}
}