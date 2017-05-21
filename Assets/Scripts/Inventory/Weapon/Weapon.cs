using UnityEngine;
using System;

[Serializable]
public class Weapon : Item
{
	[SerializeField]
	public WeaponStats weaponStats;
	private float timeToNextAttack = 0;
	public GameObject fist;

	public virtual void Initialize(string description, WeaponStats stats)
	{
		_isEquipable = true;
		this.weaponStats = stats;
		base.Initialize(description, _isConsumable, _isEquipable);
	}

	public virtual void Attack(PawnBehaviour owner, PawnBehaviour target)
	{
		if(timeToNextAttack >= Time.time && Vector2.Distance(gameObject.transform.position, target.transform.position) < weaponStats.RangeOfAttack)
		{
			timeToNextAttack = Time.time + Mathf.Clamp(weaponStats.TimeBetweenAttack - owner.specs.GetStat<BaseSpec>("Strength").Value * 2f / owner.specs.GetStat<BaseSpec>("Strength").MaxValue, 0.5f, weaponStats.TimeBetweenAttack);
			
		}
	}

	public override void Equip(PawnBehaviour owner)
	{
		if (_isEquipable)
		{
			owner.EquipedWeapon = this;
			owner.inventory.RemoveItem(this);
		}
	}

	public Weapon Fist()
	{
		return Instantiate(fist).GetComponent<Weapon>();
	}
}
