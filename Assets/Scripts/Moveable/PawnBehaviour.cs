using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class PawnBehaviour : MonoBehaviour
{
	public Inventory inventory;
	public SpecsHolder specs;

	[SerializeField]
	public Weapon EquipedWeapon;

	[SerializeField]
	public PawnBehaviour EnemyTarget;

	private bool isDead = false;

	[SerializeField]
	private Transform weaponSlot;

	public bool IsDead
	{
		get { return isDead; }
		set { isDead = value; }
	}

	protected void Awake()
	{
		inventory = GetComponent<Inventory>();
		specs = GetComponent<SpecsHolder>();
	}

	public virtual void Attack(PawnBehaviour target)
	{
		if (EquipedWeapon != null)
		{
			EquipedWeapon.Attack(this, target);
		}
	}

	public virtual void Use(Item item)
	{
		specs.AddEffects(item.effects);
		inventory.RemoveItem(item);
	}

	public virtual void Equip(Item item)
	{
		if (item is Weapon)
		{
			if (EquipedWeapon != null)
			{
				inventory.AddItem(EquipedWeapon);
				EquipedWeapon = item as Weapon;
			}
			else
			{
				EquipedWeapon = item as Weapon;
			}
			inventory.RemoveItem(item);
			EquipedWeapon.transform.SetParent(weaponSlot);
		}
	}

	public virtual void TakeDamage(float value, List<WeaponEffect> effects)
	{
		specs.GetStat<BaseAttribute>("Health").FinalValue -= value;
		specs.AddEffects(effects);
	}

	public virtual void Die()
	{
		
	}
}
