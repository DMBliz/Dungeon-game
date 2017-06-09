using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Weapon : Item
{
	[SerializeField]
	public WeaponStats weaponStats;
	private float timeToNextAttack;
	public GameObject fist;
	public Animator anim;
	private bool isAttacking = false;

	protected override void Awake()
	{
		base.Awake();
		timeToNextAttack = 0;
	}

	public virtual void Initialize(string description, WeaponStats stats, Animator anim)
	{
		this.weaponStats = stats;
		base.Initialize(description);
		IsEquipable = true;
		this.anim = anim;
	}

	void Update()
	{
		if (timeToNextAttack >= 0)
		{
			timeToNextAttack -= Time.deltaTime;
			if (anim != null && !isAttacking) 
			{
				anim.SetBool("isAttacking", false);
				anim.SetFloat("attackState", timeToNextAttack);
			}
		}
		if (isAttacking)
		{
			isAttacking = !isAttacking;
		}
	}

	public virtual void Attack(PawnBehaviour owner, PawnBehaviour target)
	{
		if (timeToNextAttack <= 0 && Vector2.Distance(owner.transform.position, target.transform.position) < weaponStats.RangeOfAttack)
		{
			target.TakeDamage(weaponStats.GetDamage, weaponStats.Effects);
			if (anim != null)
			{
				anim.SetBool("isAttacking", true);
				isAttacking = true;
			}
			timeToNextAttack = weaponStats.TimeBetweenAttack;
		}
	}

	public void PlayerAttack(PawnBehaviour owner, float angle)
	{
		if (timeToNextAttack <= 0)
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(owner.transform.position, weaponStats.RangeOfAttack);
			foreach (Collider2D collider in colliders)
			{
				if (collider.tag == owner.enemyTag)
				{
					Vector2 vec = collider.transform.position - owner.transform.position;
					float curAngle = Vector2.Angle(owner.transform.up, vec);
					if (curAngle < angle / 2f)
					{
						PawnBehaviour temp = collider.GetComponent<PawnBehaviour>();
						if (temp != null)
						{
							temp.TakeDamage(weaponStats.GetDamage, weaponStats.Effects);
						}
					}
					print(collider.tag + ":" + LayerMask.LayerToName(collider.gameObject.layer) + ":" + angle);
				}
				
				
			}
			if (anim != null)
			{
				anim.SetBool("isAttacking", true);
				isAttacking = true;
			}
			timeToNextAttack = weaponStats.TimeBetweenAttack;
		}
	}

	public Weapon Fist()
	{
		return Instantiate(fist).GetComponent<Weapon>();
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position,weaponStats.RangeOfAttack);
	}
}
