using UnityEngine;
using System.Threading;
using System;

[Serializable]
public class TimeChangeSpecModificator : BaseSpecModificator
{
	public delegate void ValueChange(float value);
	public event ValueChange ValueChangeEvent;

	public delegate void TimeEndDelegate(TimeChangeSpecModificator modificator);
	public event TimeEndDelegate TimeEndEvent;

	delegate void ChangeDeleaget();
	ChangeDeleaget MakeChange;
	
	public float startTime;
	public float time;
	public float doneChanges;
	public float timeBetweenChange;

	public TimeChangeSpecModificator(string description, float value, EStatModificatorType type, float time,float timeBetweeChange):base(description,value,type)
	{
		doneChanges = value;
		this.time = time;
		this.startTime = Time.time;
		this.timeBetweenChange = timeBetweeChange;
		MakeChange += ValueChanger;
		MakeChange.BeginInvoke(new AsyncCallback(TimeEnd), null);
	}

	public TimeChangeSpecModificator(string name, string description, float value, EStatModificatorType type, float time, float timeBetweeChange) : base(name, description, value, type)
	{
		doneChanges = value;
		this.time = time;
		this.startTime = Time.time;
		this.timeBetweenChange = timeBetweeChange;
		MakeChange += ValueChanger;
		MakeChange.BeginInvoke(new AsyncCallback(TimeEnd), null);
	}

	private void ValueChanger()
	{
		while (time + startTime >= Time.time)
		{
			value += doneChanges;
			ValueChangeEvent(value);
			Thread.Sleep(TimeSpan.FromSeconds(timeBetweenChange));
		}
	}

	public void TimeEnd(IAsyncResult res)
	{
		TimeEndEvent(this);
	}

}
