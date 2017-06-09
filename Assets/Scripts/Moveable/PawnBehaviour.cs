using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class PawnBehaviour : MonoBehaviour
{
	public Inventory inventory;
	public SpecsHolder specs;
	public string enemyTag;

	[SerializeField]
	public Weapon EquipedWeapon;

	[SerializeField]
	public PawnBehaviour EnemyTarget;

	private bool isDead = false;

	[SerializeField]
	protected Transform weaponSlot;

	public bool IsDead
	{
		get { return isDead; }
		set { isDead = value; }
	}

	protected void Awake()
	{
		inventory = GetComponent<Inventory>();
		specs = GetComponent<SpecsHolder>();
		specs.AddSpec(new BaseAttribute("Health", "This is health", 100));
		specs.GetStat<BaseAttribute>("Health").OnZero += Die;
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
		inventory.DestroyItem(item);
	}

	public virtual void Equip(Item item)
	{
		if (item is Weapon)
		{
			if (EquipedWeapon != null)
			{
				inventory.AddItem(EquipedWeapon);
				EquipedWeapon.transform.SetParent(null);
			}
			if(inventory.Contains(item))
				inventory.RemoveItem(item);
			EquipedWeapon = (Weapon) item;
			EquipedWeapon.transform.SetParent(weaponSlot);
			EquipedWeapon.transform.localPosition=Vector3.zero;
			EquipedWeapon.transform.localRotation=Quaternion.identity;
			EquipedWeapon.GetComponent<Collider2D>().enabled = false;
			EquipedWeapon.anim = GetComponent<Animator>();
		}
	}

	public virtual void TakeDamage(float value, List<WeaponEffect> effects)
	{
		if (!isDead)
		{
			specs.GetStat<BaseAttribute>("Health").FinalValue += value;
			if (effects != null)
				specs.AddEffects(effects);
			DamagePopUpManager.instance.PopUpDamge(Mathf.Abs(value).ToString("####"), transform.position);
		}
	}

	protected  virtual void Die()
	{
		isDead = true;
		EquipedWeapon.MoveToWorld(weaponSlot.transform.position);
		EquipedWeapon.transform.SetParent(null);
		EquipedWeapon = null;
	}
}
