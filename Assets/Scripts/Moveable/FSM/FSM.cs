using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FSM : MonoBehaviour
{
	private Stack<Action> states = new Stack<Action>();

	void Update()
	{
		GetCurrentState().Invoke();
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
		return states.Peek();
	}
}
