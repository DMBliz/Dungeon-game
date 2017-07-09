using UnityEngine;
using System.Collections.Generic;

public class Modificators : MonoBehaviour
{
	public static Modificators instance;

	[SerializeField]
	private List<BaseModificator> baseModificators = new List<BaseModificator>();
	[SerializeField]
	private List<TimeModificator> timeModificators = new List<TimeModificator>();
	[SerializeField]
	private List<TimeChangeModificator> timeChangeModificators = new List<TimeChangeModificator>();

	void Awake()
	{
		instance = this;
	}

	public BaseModificator GetModificator(string name)
	{
		foreach (BaseModificator modificator in baseModificators)
		{
			if (modificator.Name == name)
			{
				return modificator;
			}
		}

		foreach (TimeModificator modificator in timeModificators)
		{
			if (modificator.Name == name)
			{
				return modificator;
			}
		}

		foreach (TimeChangeModificator modificator in timeChangeModificators)
		{
			if (modificator.Name == name)
			{
				return modificator;
			}
		}
		return null;
	}

	public BaseModificator GetModificator(int id)
	{
		foreach (BaseModificator modificator in baseModificators)
		{
			if (modificator.Id == id)
			{
				return modificator;
			}
		}

		foreach (TimeModificator modificator in timeModificators)
		{
			if (modificator.Id == id)
			{
				return modificator;
			}
		}

		foreach (TimeChangeModificator modificator in timeChangeModificators)
		{
			if (modificator.Id == id)
			{
				return modificator;
			}
		}
		return null;
	}
}
