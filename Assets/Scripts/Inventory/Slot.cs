using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
	public InventoryUI containerUI;
	[SerializeField]
	private ItemUI item;

	public ItemUI ItemUi
	{
		get { return item; }
		set
		{
			item = value;
			if (item != null)
			{
				item.transform.position = transform.position;
				if (item.currentSlot != this)
					item.currentSlot = this;
			}
		}
	}

	public void OnDrop(PointerEventData eventData)
	{
		ItemUI newItem = eventData.pointerDrag.GetComponent<ItemUI>();
		if (newItem != null)
		{
			newItem.SetSlot(this);
		}
	}

	public void Clear()
	{
		Destroy(item.gameObject);
		item = null;
	}
}
