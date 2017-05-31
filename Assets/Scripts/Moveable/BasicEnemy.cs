using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : PawnBehaviour
{
	[SerializeField]
	private EnemyFSM fsm;

	public EnemyFSM Fsm
	{
		get { return fsm; }
	}

	new void Awake()
	{
		base.Awake();
		fsm.Init(GetComponent<FSM>(),this);
	}
	
	void OnDrawGizmos()
	{
		fsm.DrawGizmos();
	}
}