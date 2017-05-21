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

	public void SetCapacity(int count)
	{
		maxCapacity = count;
	}

	public bool AddItem(Item item)
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
			return true;
		}
		return false;
	}

	public bool RemoveItem(Item item)
	{
		if (_items.Count == 0)
		{
			return false;
		}

		if (_items.Contains(item))
		{
			_items.Remove(item);
			item.OnItemDrop -= DropItem;
			item.OnItemTransfer -= TransferItem;
			if (OnRemoveItem != null)
			{
				OnRemoveItem(item);
			}
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
