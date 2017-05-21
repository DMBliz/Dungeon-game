using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PawnBehaviour
{
	public static Player player;

	public GameObject someItemPrefab;
	public GameObject someItemPrefab2;
	public GameObject someItemPrefab3;

	private Inventory findedInventory;
	private Item findedItem;

	private bool inInteraction = false;

	private float SearchRadius=3f;

	new void Awake()
	{
		base.Awake();
		player = this;

		specs.AddStat(new MainSpec("Mana", "Your mana", 100));
		specs.AddStat(new MainSpec("Stamina", "Your Stamina", 100));
		specs.AddStat(new BaseSpec("Strength", "Your strength", 5,10));
	}

	void Start()
	{
		inventory = gameObject.GetComponent<Inventory>();
		inventory.AddItem(Instantiate(someItemPrefab).GetComponent<Item>());
		inventory.AddItem(Instantiate(someItemPrefab2).GetComponent<Item>());
		inventory.AddItem(Instantiate(someItemPrefab3).GetComponent<Item>());
		inventory.AddItem(Instantiate(someItemPrefab).GetComponent<Item>());
		StartCoroutine(FindInventory());
	}

	IEnumerator FindInventory()
	{
		UIManager.instance.OnPressInteractionButton += Interact;
		while (true)
		{
			yield return new WaitForSecondsRealtime(0.2f);
			if (inInteraction)
			{
				if (Vector2.Distance(findedInventory.transform.position, transform.position) > SearchRadius)
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
				for (int i = 0; i < things.Length; i++)
				{
					Inventory temp = things[i].GetComponent<Inventory>();
					if (temp != null && temp != inventory) 
					{
						findedInventory = things[i].GetComponent<Inventory>();
						break;
					}
					if (things[i].GetComponent<Item>() != null)
					{
						findedItem = things[i].GetComponent<Item>();
						break;
					}
				}

				if (findedInventory != null)
				{
					UIManager.instance.ShowTextInfo("Press E to Loot inventory");
				}
				else if (findedItem != null)
				{
					UIManager.instance.ShowTextInfo("Press E to pick Up item");
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
			inInteraction = false;
		}
		else
		{
			if (findedInventory != null)
			{
				inInteraction = true;
				UIManager.instance.SetUIInventory(findedInventory);
			}
			else if (findedItem != null)
			{
				inventory.AddItem(findedItem);
				findedItem = null;
			}

			UIManager.instance.HideTextInfo();
		}
	}

	public void Loot(Inventory lootTarget)
	{
		UIManager.instance.SetUIInventory(lootTarget);
	}

	public override void Use(Item item)
	{
		specs.AddModificator(item.modificators);
		inventory.RemoveItem(item);
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
