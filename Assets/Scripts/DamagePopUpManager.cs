using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUpManager : MonoBehaviour
{

	public static DamagePopUpManager instance;

	[SerializeField]
	private GameObject popUpTextprefab;

	void Awake()
	{
		instance = this;
	}

	public void PopUpDamge(string damage, Vector3 position)
	{
		GameObject parent = Instantiate(popUpTextprefab);
		parent.transform.SetParent(transform, false);
		parent.transform.GetChild(0).GetComponent<Text>().text = damage;
		parent.transform.position = Camera.main.WorldToScreenPoint(new Vector2(position.x + Random.Range(-.5f, .5f), position.y + Random.Range(-.5f, .5f)));
	}
}
