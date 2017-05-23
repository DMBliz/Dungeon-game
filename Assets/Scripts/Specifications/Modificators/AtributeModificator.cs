using UnityEngine;
using System;
using System.Threading;

public class AtributeModificator : IModificator
{
	public event Action<IModificator> OnEnd;
	public Action a { get; set; }
	private string name;
	private string description;
	private int value;
	private float time;
	private Atribute atr;

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

	public AtributeModificator(string name, string description, int value, float time,Atribute atr)
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
		atr.CurrentValue += value;
		Thread.Sleep(TimeSpan.FromSeconds(endTime));
		if (OnEnd != null)
		{
			OnEnd(this);
			a.EndInvoke(null);
		}
		atr.CurrentValue -= value;
	}
}

