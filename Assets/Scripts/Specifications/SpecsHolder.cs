using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SpecsHolder
{
	[SerializeField]
	List<MainSpec> mainSpecs = new List<MainSpec>();
	[SerializeField]
	List<BaseSpec> specs = new List<BaseSpec>();

	public SpecsHolder()
	{
		
	}

	public bool AddStat<T>(T stat)
	{
		if (typeof(T) == typeof(MainSpec))
		{
			if (mainSpecs.Contains(stat as MainSpec))
			{
				return false;
			}
			else
			{
				mainSpecs.Add(stat as MainSpec);
				return true;
			}
		}
		else if (typeof(T) == typeof(BaseSpec))
		{
			if (specs.Contains(stat as BaseSpec))
			{
				return false;
			}
			else
			{
				specs.Add(stat as BaseSpec);
				return true;
			}
		}
		return false;
	}

	public bool RemoveStat<T>(T stat)
	{
		if (typeof(T) == typeof(MainSpec))
		{
			if (mainSpecs.Contains(stat as MainSpec))
			{
				mainSpecs.Add(stat as MainSpec);
				return true;
			}
			else
			{
				return false;
			}
		}
		else if (typeof(T) == typeof(BaseSpec))
		{
			if (specs.Contains(stat as BaseSpec))
			{
				specs.Add(stat as BaseSpec);
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	public T GetStat<T>(string name) where T : class
	{
		if(typeof(T) == typeof(MainSpec))
		{
			foreach (var item in mainSpecs)
			{
				if (item.name == name)
					return item as T;
			}
		}
		else if(typeof(T) == typeof(BaseSpec))
		{
			foreach (var item in specs)
			{
				if (item.name == name)
					return item as T;
			}
		}
		return null;
	}

	public void AddModificator(string name, BaseSpecModificator modificator)
	{
		foreach (var item in mainSpecs)
		{
			if (item.name == name)
			{
				item.AddModificator(modificator);
			}
		}


		foreach (var item in specs)
		{
			if (item.name == name)
			{
				item.AddModificator(modificator);
			}
		}
	}

	public void AddModificator(Dictionary<string, BaseSpecModificator> mods)
	{
		foreach (var mod in mods)
		{
			foreach (var stat in mainSpecs)
			{
				if (stat.name == mod.Key)
				{
					stat.AddModificator(mod.Value);
					break;
				}
			}
		}

		foreach (var mod in mods)
		{
			foreach (var stat in specs)
			{
				if (stat.name == mod.Key)
				{
					stat.AddModificator(mod.Value);
					break;
				}
			}
		}
	}
}
