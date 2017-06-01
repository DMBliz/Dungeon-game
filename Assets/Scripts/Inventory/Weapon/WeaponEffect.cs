using System;
using UnityEngine;

[Serializable]
public class WeaponEffect : Effect
{
	[Range(0,100)]
	public float chance;

	public WeaponEffect(string statName, int id, float chance) : base(statName, id)
	{
		this.chance = chance;
	}

	public WeaponEffect(string statName, string name, float chance) : base(statName, name)
	{
		this.chance = chance;
	}
}
