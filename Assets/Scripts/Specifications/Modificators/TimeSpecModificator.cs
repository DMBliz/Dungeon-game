using System;
using System.Threading;
using UnityEngine;

[Serializable]
public abstract class TimeSpecModificator : BaseSpecModificator
{
	public delegate void TimeChecker(TimeSpecModificator mod);
	public delegate void Checker();
	public Checker chek;
	public event TimeChecker timeEnd;

	[SerializeField]
	public float time;
	[SerializeField]
	public float startTime;

	public TimeSpecModificator(string description, float value, EStatModificatorType ModificatorType, float time) :base(description, value, ModificatorType)
	{
		this.time = time;
		this.startTime = Time.time;
		chek += CheckTime;
		chek.BeginInvoke(null, null);
	}

	public TimeSpecModificator(string name, string description, float value, EStatModificatorType ModificatorType, float time) : base(name, description, value, ModificatorType)
	{
		this.time = time;
		this.startTime = Time.time;
		chek += CheckTime;
		chek.BeginInvoke(null, null);
	}

	public virtual void CheckTime()
	{
		Thread.Sleep(TimeSpan.FromSeconds(Time.time + time));
		while(startTime + time >= Time.time)
		{
			if (timeEnd != null)
			{
				timeEnd(this);
			}
		}
	}
}
