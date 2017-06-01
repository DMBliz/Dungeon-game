using System;
using UnityEngine;
using System.Collections;
using System.Threading;

[Serializable]
public class TimeModificator : BaseModificator
{
	public event Action<TimeModificator> OnTimeEnd;
	protected Action worker;
	[SerializeField]
	protected int totalTime;
	protected int endTime;
	[SerializeField]
	private bool isMax;

	public bool IsMax
	{
		get { return isMax; }
		set { isMax = value; }
	}

	public TimeModificator(string name, string description, ModificatorType type, float value, int totalTime,bool isMax=false) : base(name, description, type, value)
	{
		this.totalTime = totalTime;
		this.isMax = isMax;
	}

	public virtual void Start()
	{
		worker = Process;
		endTime = (int)Time.time + totalTime;
		worker.BeginInvoke(OnEnd, null);
	}

	protected virtual void Process()
	{
		Thread.Sleep(TimeSpan.FromSeconds(totalTime));
		while (true)
		{
			if (endTime > Time.time)
				break;
		}
	}

	protected virtual void OnEnd(IAsyncResult res)
	{
		if (OnTimeEnd != null)
			OnTimeEnd(this);
	}
}
