using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
	[SerializeField]
	private Inventory _inventory;

	public Inventory inventory { get { return _inventory; } set { UnsubcribeEvents(); _inventory = value; InventorySet(); } }

	private Text Title;

	public GameObject inventoryItemPrefab;

	private List<Slot> slots = new List<Slot>();

	void Awake()
	{
		Title = transform.parent.GetChild(0).GetComponent<Text>();
	}

	void Start()
	{
		InventorySet();
	}

	private void InventorySet()
	{
		UpdateSlots();
		CheckItem();
		if (_inventory != null)
		{
			_inventory.OnAddItem += AddItem;
			_inventory.OnRemoveItem += RemoveItem;
		}
	}

	private void CheckItem()
	{
		if (inventory != null)
		{
			foreach (Item t in inventory.items)
			{
				AddItem(t);
			}
		}
	}

	private void UnsubcribeEvents()
	{
		if (_inventory != null)
		{
			_inventory.OnAddItem -= AddItem;
			_inventory.OnRemoveItem -= RemoveItem;
		}
	}

	void UpdateSlots()
	{
		slots.Clear();
		for (int i = 0; i < transform.childCount; i++)
		{
			slots.Add(transform.GetChild(i).GetComponent<Slot>());
			slots[i].containerUI = this;
		}
	}

	private Slot GetSlotByItem(Item item)
	{
		for (int i = 0; i < slots.Count; i++)
		{
			if (slots[i].ItemUi.StoredItem == item)
			{
				return slots[i];
			}
		}
		return null;
	}

	public void SetTitle(string title)
	{
		Title.text = title;
	}

	private void RemoveItem(Item item)
	{
		Slot slot = GetSlotByItem(item);
		if (slot != null)
		{
			slot.Clear();
		}
	}

	private void AddItem(Item item)
	{
		if (!Contains(item))
		{
			for (int i = 0; i < slots.Count; i++)
			{
				if (slots[i].ItemUi == null)
				{
					slots[i].ItemUi = Instantiate(inventoryItemPrefab).GetComponent<ItemUI>();
					slots[i].ItemUi.StoredItem = item;
					return;
				}
			}
		}
	}

	public bool Contains(Item item)
	{
		for (int i = 0; i < slots.Count; i++)
		{
			if (slots[i].ItemUi != null && slots[i].ItemUi.StoredItem == item)
			{
				return true;
			}
		}
		return false;
	}
}
