using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Item item;
	private Slot _currentSlot;
	private Slot newSlot;

	public Item StoredItem
	{
		get { return item; }
		set
		{
			item = value;
			if (item != null)
				gameObject.GetComponent<Image>().sprite = item.UISprite;		
		}
	}

	public Slot currentSlot
	{
		get { return _currentSlot; }
		set
		{
			_currentSlot = value;
			transform.SetParent(_currentSlot.transform);
			if(_currentSlot.ItemUi != this)
				_currentSlot.ItemUi = this;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
		transform.SetParent(_currentSlot.transform.parent);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position;
	}

	public void SetSlot(Slot slot)
	{
		newSlot = slot;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (newSlot != null)
		{
			if (newSlot.ItemUi != null && newSlot.ItemUi.StoredItem != null) 
			{
				GetComponent<CanvasGroup>().blocksRaycasts = true;

				if (currentSlot.containerUI != newSlot.containerUI)
				{
					newSlot.ItemUi.item.Transfer(currentSlot.containerUI.inventory);
					item.Transfer(newSlot.containerUI.inventory); 
				}

				newSlot.ItemUi.currentSlot = currentSlot;
				currentSlot = newSlot;
				newSlot = null;
			}
			else
			{
				GetComponent<CanvasGroup>().blocksRaycasts = true;

				if (currentSlot.containerUI.inventory != newSlot.containerUI.inventory)
				{
					item.Transfer(newSlot.containerUI.inventory);
				}
				currentSlot.ItemUi = null;
				currentSlot = newSlot;
				newSlot = null;
			}
		}
		else
		{
			Drop();
		}
	}

	public void Equip()
	{
		currentSlot.containerUI.inventory.GetComponent<PawnBehaviour>().Equip(item);
		UnSubcribeActions();
	}

	public void Consume()
	{
		currentSlot.containerUI.inventory.GetComponent<PawnBehaviour>().Use(item);
		UnSubcribeActions();
	}

	public void Drop()
	{
		item.Drop();
		UnSubcribeActions();
	}

	private void UnSubcribeActions()
	{
		UIManager.instance.OnDropAction -= Drop;
		UIManager.instance.OnEquipAction -= Equip;
		UIManager.instance.OnUseAction -= Consume;
		UIManager.instance.OnNoneAction -= UnSubcribeActions;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			UIManager.instance.ShowInventoryMenu(item.IsEquipable, item.IsEquipable);
			UIManager.instance.OnDropAction += Drop;
			UIManager.instance.OnEquipAction += Equip;
			UIManager.instance.OnUseAction += Consume;
			UIManager.instance.OnNoneAction += UnSubcribeActions;

		}
		else if (eventData.button == PointerEventData.InputButton.Left)
		{
			UIManager.instance.HideInventoryMenu();
			UnSubcribeActions();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UIManager.instance.ShowInventoryTooltip(item.ToolTipDescription);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIManager.instance.HideInventoryTooltip();
	}
}
