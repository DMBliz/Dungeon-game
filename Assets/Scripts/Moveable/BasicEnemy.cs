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

	void Start()
	{
		GameObject weap = Instantiate(PrefabHolder.instance.getWeaponByName("Silver sword"));
		Equip(weap.GetComponent<Item>());
		fsm.AttackDistance = EquipedWeapon.weaponStats.RangeOfAttack;

	}
	
	void OnDrawGizmos()
	{
		fsm.DrawGizmos();
	}

	public override void TakeDamage(float value, List<WeaponEffect> effects)
	{
		base.TakeDamage(value, effects);
		print(name + " take damage:" + value + ":"+specs.GetStat<BaseAttribute>("Health").FinalValue);
	}

	protected override void Die()
	{
		base.Die();
		GetComponent<FSM>().isDead = IsDead;
		Debug.Log(name + " dead");
	}
}