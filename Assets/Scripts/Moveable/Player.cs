using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : PawnBehaviour
{
	public static Player player;

	public GameObject someItemPrefab;
	public GameObject someItemPrefab2;
	public GameObject someItemPrefab3;
	public GameObject someItemPrefab4;

	private Inventory findedInventory;
	private Item findedItem;

	[SerializeField]
	private bool inInteraction = false;
	[SerializeField]
	private float SearchRadius = 2f;
	[SerializeField]
	private float attackAngle = 2f;

	new void Awake()
	{
		base.Awake();
		player = this;
		CameraManager.instance.target = gameObject;
	}

	void Start()
	{
		inventory = gameObject.GetComponent<Inventory>();
		UIManager.instance.playerInventory.transform.GetChild(1).GetComponent<InventoryUI>().inventory = inventory;
		inventory.AddItem(Instantiate(someItemPrefab).GetComponent<Item>());
		inventory.AddItem(Instantiate(someItemPrefab2).GetComponent<Item>());
		inventory.AddItem(Instantiate(someItemPrefab3).GetComponent<Item>());
		inventory.AddItem(Instantiate(someItemPrefab).GetComponent<Item>());
		inventory.AddItem(Instantiate(someItemPrefab4).GetComponent<Item>());
		GameObject weap = Instantiate(PrefabHolder.instance.getWeaponByName("Silver sword"));
		Equip(weap.GetComponent<Item>());
		StartCoroutine(FindInventory());
		specs.OnAddModificator += UIManager.instance.AddEffect;
		specs.OnRemoveModificator += UIManager.instance.RemoveEffect;
		specs.GetStat<BaseAttribute>("Health").OnValueChange += UpdateHealth;
		
		EquipedWeapon.anim = GetComponent<Animator>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Attack(null);
		}
	}

	public override void Attack(PawnBehaviour target)
	{
		if (EquipedWeapon != null) 
		EquipedWeapon.PlayerAttack(this, attackAngle);
	}

	IEnumerator FindInventory()
	{
		UIManager.instance.OnPressInteractionButton += Interact;
		while (true)
		{
			yield return new WaitForSecondsRealtime(0.2f);
			if (inInteraction)
			{
				if(!new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, SearchRadius)).Contains(findedInventory.GetComponent<Collider2D>()))
				{
					findedInventory = null;
					UIManager.instance.HideUIInventory();
					inInteraction = false;
				}
			}
			else
			{
				findedItem = null;
				findedInventory = null;
				Collider2D[] things = Physics2D.OverlapCircleAll(transform.position, SearchRadius);
				float minDist = float.MaxValue;
				Collider2D col = null;
				for (int i = 0; i < things.Length; i++)
				{
					Inventory inv = things[i].GetComponent<Inventory>();

					if (inv == null && things[i].GetComponent<Item>() == null) 
						continue;

					if (inv != null)
					{
						PawnBehaviour pawn = inv.GetComponent<PawnBehaviour>();
						if (pawn != null)
						{
							if (pawn == this || !pawn.IsDead)
								continue;
						}
					}

					float dist = Vector2.Distance(this.transform.position, things[i].transform.position);

					if (dist < minDist)
					{
						minDist = dist;
						col = things[i];
					}
				}

				if (col != null)
				{
					findedInventory = col.GetComponent<Inventory>();
					findedItem = col.GetComponent<Item>();
				}

				if (findedInventory != null)
				{
					UIManager.instance.ShowTextInfo("Press E to Loot");
				}
				else if (findedItem != null)
				{
					UIManager.instance.ShowTextInfo("Press E to pick Up " + findedItem.itemName);
				}
				else
				{
					UIManager.instance.HideTextInfo();
				}
			}
		}
	}

	public void Interact()
	{
		if (inInteraction)
		{
			findedInventory = null;
			UIManager.instance.HideUIInventory();
			UIManager.instance.HidePlayerInventory();
			inInteraction = false;
		}
		else
		{
			if (findedInventory != null)
			{
				inInteraction = true;
				UIManager.instance.SetUIInventory(findedInventory);
				UIManager.instance.ShowPlayerInventory();
			}
			else if (findedItem != null)
			{
				inventory.AddItem(findedItem);
				findedItem = null;
			}

			UIManager.instance.HideTextInfo();
		}
	}

	void UpdateHealth(BaseAttribute attribute,float value)
	{
		BaseAttribute health = specs.GetStat<BaseAttribute>("Health");
		UIManager.instance.SetHealthBarValue(health.FinalValue, health.MaxValue);
	}

	public override void TakeDamage(float value, List<WeaponEffect> effects)
	{
		base.TakeDamage(value, effects);
		BaseAttribute health = specs.GetStat<BaseAttribute>("Health");
		UIManager.instance.SetHealthBarValue(health.FinalValue, health.MaxValue);
	}

	public void OnDrawGizmos()
	{
		if (findedInventory != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(findedInventory.transform.position,1f);
		}
		if (findedItem != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(findedItem.transform.position, 1f);
		}
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position,SearchRadius);
	}
}
