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
		this.weaponStats = stats;
		base.Initialize(description);
		IsEquipable = true;
	}

	public virtual void Attack(PawnBehaviour owner, PawnBehaviour target)
	{
		if(timeToNextAttack >= Time.time && Vector2.Distance(gameObject.transform.position, target.transform.position) < weaponStats.RangeOfAttack)
		{
			target.TakeDamage(weaponStats.GetDamage, weaponStats.Effects);
		}
	}

	public void Equip(PawnBehaviour owner)
	{
		owner.EquipedWeapon = this;
		owner.inventory.RemoveItem(this);
	}

	public Weapon Fist()
	{
		return Instantiate(fist).GetComponent<Weapon>();
	}
}
