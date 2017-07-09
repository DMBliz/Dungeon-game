using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM : MonoBehaviour
{
	private Stack<Action> states = new Stack<Action>();
	public bool isDead = false;

	void Update()
	{
		Action curState = GetCurrentState();
		if (!isDead && curState!=null)
			curState.Invoke();
	}

	public void PushState(Action state)
	{
		states.Push(state);
	}

	public void PopState()
	{
		states.Pop();
	}

	public Action GetCurrentState()
	{
		if (states.Count > 0)
			return states.Peek();
		else return null;
	}
}
