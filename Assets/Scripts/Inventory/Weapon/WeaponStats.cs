using System;
using UnityEngine;

[Serializable]
public class WeaponStats
{
	[SerializeField]
	private float rangeOfAttack;
	[SerializeField]
	private float timeBetweenAttack;
	[SerializeField]
	private float minDamage;
	[SerializeField]
	private float maxDamage;

	public float RangeOfAttack { get { return rangeOfAttack; } set { rangeOfAttack = value; } }
	public float TimeBetweenAttack { get { return timeBetweenAttack; } set { timeBetweenAttack = value; } }
	public float MinDamage { get { return minDamage; } set { minDamage = value; } }
	public float MaxDamage { get { return maxDamage; } set { maxDamage = value; } }

	public WeaponStats()
	{
		rangeOfAttack = 0;
		timeBetweenAttack = 0;
		minDamage = 0;
		maxDamage = 0;
	}

	public WeaponStats(float rangeOfAttack,float timeBetweenAttack,float minDamage,float maxDamage)
	{
		this.rangeOfAttack = rangeOfAttack;
		this.timeBetweenAttack = timeBetweenAttack;
		this.minDamage = minDamage;
		this.maxDamage = maxDamage;
	}
}
