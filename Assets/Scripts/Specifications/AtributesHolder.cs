using UnityEngine;
using System.Collections.Generic;

public class AtributesHolder : MonoBehaviour
{
	public List<IAtribute> Atributes = new List<IAtribute>();
	public List<IModificator> Modificators = new List<IModificator>();

	public AtributesHolder()
	{
	}

	public void AddAtribute(IAtribute atribute)
	{
		Atributes.Add(atribute);
	}

	public void RemoveAtribute(IAtribute atribute)
	{
		Atributes.Remove(atribute);
	}

	public void AddModificator(IModificator modificator)
	{
		Modificators.Add(modificator);
	}

	public void AddModificator(List<IModificator> modificator)
	{
		Modificators.AddRange(modificator);
	}

	public void RemoveModificator(IModificator modificator)
	{
		Modificators.Remove(modificator);
	}

	public T GetAtribute<T>(string name) where T : class
	{
		for (int i = 0; i < Atributes.Count; i++)
		{
			if (Atributes[i].Name == name)
			{
				return (T)Atributes[i];
			}
		}
		return null;
	}

	public IAtribute GetAtribute(string name)
	{
		for (int i = 0; i < Atributes.Count; i++)
		{
			if (Atributes[i].Name == name)
			{
				return Atributes[i];
			}
		}
		return null;
	}
}
