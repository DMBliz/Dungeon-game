using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	[Header("Inventory")]
	[SerializeField]
	private GameObject lootInventory;
	private InventoryUI inventoryUI;
	[SerializeField]
	private GameObject playerInventory;

	[SerializeField]
	private GameObject toolTip;
	private Text toolTipText;

	[SerializeField]
	private GameObject ActionMenu;
	private GameObject EquipMenuAction;
	private GameObject ConsumeMenuAction;

	[SerializeField]
	private GameObject interactionText;

	public event Action OnDropAction;
	public event Action OnUseAction;
	public event Action OnEquipAction;
	public event Action OnNoneAction;

	public event Action OnPressInteractionButton;

	void Awake()
	{
		instance = this;
		inventoryUI = lootInventory.transform.GetChild(1).GetComponent<InventoryUI>();

		ConsumeMenuAction = ActionMenu.transform.GetChild(0).gameObject;
		EquipMenuAction = ActionMenu.transform.GetChild(1).gameObject;

		toolTipText = toolTip.transform.GetChild(0).GetComponent<Text>();
	}

	void Start ()
	{
		//toolTipText.text = "Huge Sword \nDamage: <color=green>5</color>-<color=blue>10</color>\nSpeed: <color=red>1s</color>\n\nThis sword was..";
	}

	void Update ()
	{
		if (toolTip.activeInHierarchy)
		{
			toolTip.transform.position = Input.mousePosition;
		}

		if (ActionMenu.activeInHierarchy && Input.GetMouseButtonDown(0))
		{
			HideInventoryMenu();
			if (OnNoneAction != null)
			{
				OnNoneAction();
			}
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			if (playerInventory.activeInHierarchy)
			{
				HidePlayerInventory();
			}
			else
			{
				ShowPlayerInventory();
			}
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			if (OnPressInteractionButton != null)
			{
				OnPressInteractionButton();
			}
		}
	}

	public void ShowPlayerInventory()
	{
		playerInventory.SetActive(true);
	}

	public void HidePlayerInventory()
	{
		playerInventory.SetActive(false);
		HideInventoryTooltip();
	}

	public void SetUIInventory(Inventory owner)
	{
		lootInventory.SetActive(true);
		inventoryUI.inventory = owner;
		inventoryUI.transform.parent.GetChild(0).GetComponent<Text>().text = owner.name;
	}

	public void HideUIInventory()
	{
		lootInventory.SetActive(false);
		inventoryUI.inventory = null;
		HideInventoryTooltip();
	}

	public void ShowInventoryMenu(bool showEquip=false, bool showConsume=false)
	{
		ActionMenu.SetActive(true);
		EquipMenuAction.SetActive(showEquip);
		ConsumeMenuAction.SetActive(showConsume);
		ActionMenu.transform.position = Input.mousePosition;
		HideInventoryTooltip();
	}

	public void HideInventoryMenu()
	{
		ActionMenu.SetActive(false);
		HideInventoryTooltip();
	}

	public void ShowInventoryTooltip(string text)
	{
		if (!ActionMenu.activeInHierarchy)
		{
			toolTipText.text = text;
			toolTip.SetActive(true);
		}
	}

	public void HideInventoryTooltip()
	{
		toolTip.SetActive(false);
	}

	public void ShowTextInfo(string text)
	{
		interactionText.SetActive(true);
		interactionText.GetComponent<Text>().text = text;
	}

	public void HideTextInfo()
	{
		interactionText.SetActive(false);
		interactionText.GetComponent<Text>().text = "";
	}

	public void ActionMenuClick(int button)
	{
		switch (button)
		{
			case 0:
				if (OnDropAction != null)
					OnDropAction();
				break;
			case 1:
				if (OnUseAction != null)
					OnUseAction();
				break;
			case 2:
				if (OnEquipAction != null)
					OnEquipAction();
				break;
		}
		HideInventoryMenu();
	}
}
