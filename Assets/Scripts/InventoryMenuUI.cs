using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMenuUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public bool isHover = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && !isHover)
		{
			ButtonClicked(-1);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isHover = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isHover = false;
	}

	public void ButtonClicked(int button)
	{
		UIManager.instance.InventoryMenuSelect(button);
		isHover = false;
	}
}
