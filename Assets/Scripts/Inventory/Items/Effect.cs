using System;

[Serializable]
public class Effect
{
	public string statName;

	public BaseModificator modificator
	{
		get
		{
			if (id > 0)
			{
				return Modificators.instance.GetModificator(id);
			}
			else
			{
				return Modificators.instance.GetModificator(name);
			}
		}
	}

	public int id;
	public string name;

	public Effect(string statName, int id)
	{
		this.statName = statName;
		this.id = id;
	}

	public Effect(string statName, string name)
	{
		this.statName = statName;
		this.name = name;
	}
}
