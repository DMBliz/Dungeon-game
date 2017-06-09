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
		endTime = (int)Time.time + totalTime;
		UIManager.instance.StartCoroutine(Process());
	}

	protected virtual IEnumerator Process()
	{
		yield return new WaitForSeconds(totalTime);
		OnEnd();
	}

	protected virtual void OnEnd()
	{
		if (OnTimeEnd != null)
			OnTimeEnd(this);
	}
}
