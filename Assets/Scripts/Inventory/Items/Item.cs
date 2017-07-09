using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
	public event Action<Item> OnItemDrop;
	public event Action<Item> OnItemTransfer;

	[SerializeField]
	public string itemName;
	[SerializeField]
	public string description;
	[SerializeField]
	public int id;

	[SerializeField]
	private bool isConsumable;
	[SerializeField]
	private bool isEquipable;

	public bool IsConsumable
	{
		get { return isConsumable; }
		protected set { isConsumable = value; }
	}

	public bool IsEquipable
	{
		get { return isEquipable; }
		protected set { isEquipable = value; }
	}

	[SerializeField]
	public Sprite itemSprite;

	[SerializeField]
	public Sprite UISprite;

	[SerializeField]
	public List<Effect> effects = new List<Effect>();

	public virtual string ToolTipDescription
	{
		get { return itemName + "\n\n" + description; }
	}

	private SpriteRenderer spriteRenderer;
	private Collider2D Collider;

	protected virtual void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		Collider = GetComponent<Collider2D>();

		if (itemSprite == null && spriteRenderer.sprite != null)
		{
			itemSprite = spriteRenderer.sprite;
		}

		if (UISprite == null)
		{
			UISprite = itemSprite;
		}
	}

	public virtual void Initialize(string description)
	{
		this.itemName = GetType().ToString();
		this.description = description;
	}

	public void MoveToInventory()
	{
		spriteRenderer.enabled = false;
		Collider.enabled = false;
	}

	public void MoveToWorld(Vector2 position)
	{
		spriteRenderer.enabled = true;
		Collider.enabled = true;
		transform.position = position;
	}

	public virtual void Drop()
	{
		if (OnItemDrop != null)
		{
			OnItemDrop(this);
		}
	}

	public void ClearEvents()
	{
		OnItemTransfer = null;
		OnItemDrop = null;
	}

	public virtual void Transfer(Inventory newOwner)
	{
		if (OnItemTransfer != null)
		{
			OnItemTransfer(this);
		}
		if (newOwner != null)
		{
			newOwner.TransferItem(this);
		}
		else
		{
			Drop();
		}
	}
}
