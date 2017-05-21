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
    public Dictionary<string, BaseSpecModificator> modificators = new Dictionary<string, BaseSpecModificator>();
	[SerializeField]
	protected bool _isConsumable = false;
	[SerializeField]
	protected bool _isEquipable = false;
	public bool isConsumable { get { return _isConsumable; } }
	public bool IsEquipable { get { return _isEquipable; } }

	[SerializeField]
	Sprite itemSprite;

	[SerializeField]
	public Sprite UISprite;

	[SerializeField]
	public string itemName;
	[SerializeField]
	public string description;
	[SerializeField]
	public int id;

	public virtual string ToolTipDescription
	{
		get { return itemName + "\n\nDescription:" + description; }
	}

	private SpriteRenderer spriteRenderer;
	private Collider2D collider;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<Collider2D>();
	}

	public virtual void Initialize(string name, string description, bool isConsumable=false, bool isEquipable=false)
	{
		this.itemName = name;
		this.description = description;
		this._isConsumable = isConsumable;
		this._isEquipable = isEquipable;
	}

	public virtual void Initialize(string description, bool isConsumable = false, bool isEquipable = false)
	{
		this.itemName = GetType().ToString();
		this.description = description;
		this._isConsumable = isConsumable;
		this._isEquipable = isEquipable;
	}

	public virtual void Consume(PawnBehaviour owner)
	{
		if (_isConsumable)
		{
			owner.Use(this);
			Destroy(gameObject);
		}
	}


    public virtual void Equip(PawnBehaviour owner)
    {
		if (_isEquipable)
		{
			owner.Equip(this);
			spriteRenderer.sprite = itemSprite;
		}
    }

	public void MoveToInventory()
	{
		spriteRenderer.enabled = false;
		collider.enabled = false;
	}

	public void MoveToWorld(Vector2 position)
	{
		spriteRenderer.enabled = true;
		collider.enabled = true;
		transform.position = position;
	}

	public virtual void Drop()
	{
		if (OnItemDrop != null)
		{
			OnItemDrop(this);
		}

		//TODO: realise item drop to the world
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
