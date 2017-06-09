using UnityEngine;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
	public event Action<Item> OnAddItem;
	public event Action<Item> OnRemoveItem;

	[SerializeField]
	protected List<Item> _items = new List<Item>();
	public List<Item> items { get { return _items; } }
	[SerializeField]
	int maxCapacity = 5;
	public int MaxCapacity { get { return maxCapacity; } protected set { maxCapacity = value; } }

	public void AddItem(Item item)
	{
		if (_items.Count - 1 < maxCapacity)
		{
			_items.Add(item);
			item.OnItemDrop += DropItem;
			item.OnItemTransfer += TransferItem;
			item.MoveToInventory();
			if (OnAddItem != null)
			{
				OnAddItem(item);
			}
		}
	}

	public void RemoveItem(Item item)
	{
		if (_items.Count == 0)
		{
			return;
		}

		if (_items.Contains(item))
		{
			_items.Remove(item);
			item.OnItemDrop -= DropItem;
			item.OnItemTransfer -= TransferItem;
			item.MoveToWorld(transform.position);
			if (OnRemoveItem != null)
			{
				OnRemoveItem(item);
			}
		}
	}

	public void DestroyItem(Item item)
	{
		if (_items.Contains(item))
		{
			_items.Remove(item);
			item.OnItemDrop -= DropItem;
			item.OnItemTransfer -= TransferItem;
			item.MoveToWorld(transform.position);
			if (OnRemoveItem != null)
			{
				OnRemoveItem(item);
			}
		}

		Destroy(item.gameObject);
	}

	public bool Contains(Item item)
	{
		foreach (Item invItem in items)
		{
			if (item == invItem)
				return true;
		}
		return false;
	}

	public void DropItem(Item item)
	{
		RemoveItem(item);
		item.MoveToWorld(gameObject.transform.position);
	}

	public void TransferItem(Item item)
	{
		if (_items.Contains(item))
		{
			_items.Remove(item);
			item.OnItemDrop -= DropItem;
			item.OnItemTransfer -= TransferItem;
		}
		else
		{
			_items.Add(item);
			item.OnItemDrop += DropItem;
			item.OnItemTransfer += TransferItem;
		}
	}
}
