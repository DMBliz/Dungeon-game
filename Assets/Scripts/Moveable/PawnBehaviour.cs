using System.Collections.Generic;
using System;
using UnityEngine;

//[RequireComponent(typeof(Weapon))]
//[RequireComponent(typeof(Inventory))]
public abstract class PawnBehaviour : MonoBehaviour
{
	public Inventory inventory;
	[SerializeField]
	public SpecsHolder specs = new SpecsHolder();

	[SerializeField]
	public Weapon EquipedWeapon;

	[SerializeField]
	public PawnBehaviour EnemyTarget;

	protected void Awake()
	{
		inventory = GetComponent<Inventory>();
		specs.AddStat(new MainSpec("Health", "Your health", 100));
		specs.GetStat<MainSpec>("Health").OnValueZero += Die;
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
		specs.AddModificator(item.modificators);

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
		}
	}

	public virtual void TakeDamage(float value)
	{
		specs.GetStat<MainSpec>("Health").ValueChange(value);
	}

	public virtual void Die()
	{
		
	}
}
