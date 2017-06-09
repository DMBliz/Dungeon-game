using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponStats
{
	[SerializeField]
	private float rangeOfAttack;
	[SerializeField]
	private bool targetAttack;
	[SerializeField]
	private float timeBetweenAttack;
	[SerializeField]
	private float minDamage;
	[SerializeField]
	private float maxDamage;
	[SerializeField]
	private List<WeaponEffect> effects = new List<WeaponEffect>();

	public float RangeOfAttack { get { return rangeOfAttack; } set { rangeOfAttack = value; } }
	public bool TargetAttack { get { return targetAttack; } set { targetAttack = value; } }
	public float TimeBetweenAttack { get { return timeBetweenAttack; } set { timeBetweenAttack = value; } }
	public float MinDamage { get { return minDamage; } set { minDamage = value; } }
	public float MaxDamage { get { return maxDamage; } set { maxDamage = value; } }
	public float GetDamage { get { return -UnityEngine.Random.Range(minDamage, maxDamage); } }
	public List<WeaponEffect> Effects {get { return effects; } }

	public WeaponStats()
	{
		rangeOfAttack = 0;
		targetAttack = true;
		timeBetweenAttack = 0;
		minDamage = 0;
		maxDamage = 0;
	}

	public WeaponStats(float rangeOfAttack, float attackAngle, bool targetAttak, float timeBetweenAttack, float minDamage, float maxDamage)
	{
		this.rangeOfAttack = rangeOfAttack;
		this.targetAttack = targetAttak;
		this.timeBetweenAttack = timeBetweenAttack;
		this.minDamage = minDamage;
		this.maxDamage = maxDamage;
	}
}
