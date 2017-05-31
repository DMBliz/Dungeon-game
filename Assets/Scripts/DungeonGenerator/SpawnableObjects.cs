using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class SpawnableObjects
{
	public ObjectGroup containers;
	public ObjectGroup items;
	//public ObjectGroup furniture;

	public List<Thing> ThingsByPercent(float percent)
	{
		List<Thing> ret = new List<Thing>();

		if (percent <= containers.GroupChance)
		{
			foreach (Thing container in containers.spawnableObjects)
			{
				if (percent <= container.chance) 
				{
					ret.Add(container);
				}
			}
		}

		if (percent <= items.GroupChance)
		{
			foreach (Thing item in items.spawnableObjects)
			{
				if (percent <= item.chance)
				{
					ret.Add(item);
				}
			}
		}

		/*if (percent >= furniture.GroupChance)
		{
			foreach (Thing furn in furniture.spawnableObjects)
			{
				if (furn.chance <= percent)
				{
					ret.Add(furn);
				}
			}
		}*/
		return ret;
	}
}
