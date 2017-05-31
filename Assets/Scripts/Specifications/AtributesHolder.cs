using UnityEngine;
using System.Collections.Generic;

public class AtributesHolder : MonoBehaviour
{
	[SerializeField]
	private List<AtributeF> mainAtributes=new List<AtributeF>();
	[SerializeField]
	private List<Atribute> additAtributes=new List<Atribute>();
	public List<IModificator> Modificators = new List<IModificator>();

	public void AddAtribute(IAtribute atribute)
	{
		if (atribute is Atribute)
		{
			additAtributes.Add(atribute as Atribute);
		}
		else if(atribute is AtributeF)
		{
			mainAtributes.Add(atribute as AtributeF);
		}
	}

	public void RemoveAtribute(IAtribute atribute)
	{
		if (atribute is Atribute)
		{
			additAtributes.Remove(atribute as Atribute);
		}
		else if (atribute is AtributeF)
		{
			mainAtributes.Remove(atribute as AtributeF);
		}
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
		if (typeof(T)==typeof(Atribute))
		{
			foreach (Atribute atr in additAtributes)
			{
				if (atr.Name == name)
				{
					return atr as T;
				}
			}
		}
		else if (typeof(T) == typeof(AtributeF))
		{
			foreach (AtributeF atr in mainAtributes)
			{
				if (atr.Name == name)
				{
					return atr as T;
				}
			}
		}
		return null;
	}

	public IAtribute GetAtribute(string name)
	{
		foreach (AtributeF atr in mainAtributes)
		{
			if (atr.Name == name)
			{
				return atr;
			}
		}

		foreach (Atribute atr in additAtributes)
		{
			if (atr.Name == name)
			{
				return atr;
			}
		}
		return null;
	}
}
