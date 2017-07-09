using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonDoor : MonoBehaviour
{
	[SerializeField]
	private bool isEnter;

	public bool IsEnter
	{
		get { return isEnter; }
		set { isEnter = value; }
	}

	public void LoadLevel()
	{
		if (isEnter)
		{
			if (Level.currentlevel > 0)
			{
				Level.currentlevel--;
				Level.wasHere = true;
			}
			else
				return;
		}
		else
		{
			Level.currentlevel++;
			Level.wasHere = false;
		}
		if (Level.currentlevel < Level.levels.Count)
		{
			SceneManager.LoadScene(1);
		}
		else
		{
			UIManager.instance.Win();
		}
	}
}
