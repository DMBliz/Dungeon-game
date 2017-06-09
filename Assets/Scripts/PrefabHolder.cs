using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabHolder : MonoBehaviour
{
	public static PrefabHolder instance;
	[SerializeField]
	public List<GameObject> Weapons;
	[SerializeField]
	public List<GameObject> Foods;
	[SerializeField]
	public List<GameObject> Poutions;

	void Awake()
	{
		instance = this;
	}

	public GameObject getWeaponByName(string name)
	{
		foreach (GameObject weapon in Weapons)
		{
			if (weapon.GetComponent<Weapon>().itemName == name)
				return weapon;
		}
		return null;
	}
}
