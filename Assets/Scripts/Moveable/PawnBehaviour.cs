using System.Collections.Generic;
using System;
using UnityEngine;

//[RequireComponent(typeof(Weapon))]
//[RequireComponent(typeof(Inventory))]
public abstract class PawnBehaviour : MonoBehaviour
{
	public Inventory inventory;
	[SerializeField]
	public AtributesHolder attributes;

	[SerializeField]
	public Weapon EquipedWeapon;

	[SerializeField]
	public PawnBehaviour EnemyTarget;

	private bool isDead = false;

	public bool IsDead
	{
		get { return isDead; }
		set { isDead = value; }
	}

	protected void Awake()
	{
		inventory = GetComponent<Inventory>();
		attributes = GetComponent<AtributesHolder>();
		attributes.AddAtribute(new AtributeF("Health", "Your health", 100));
		attributes.GetAtribute("Health").OnZero += Die;
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
		attributes.AddModificator(item.modificators);

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
		attributes.GetAtribute<AtributeF>("Health").CurrentValue -= value;
	}

	public virtual void Die()
	{
		
	}
}
