using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	[SerializeField]
	private GameObject menu;
	[SerializeField]
	private GameObject UI;
	[SerializeField]
	private GameObject controls;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (menu.activeInHierarchy)
			{
				menu.SetActive(false);
				UI.SetActive(true);
				Time.timeScale = 1f;
			}
			else
			{
				menu.SetActive(true);
				UI.SetActive(false);
				Time.timeScale = 0f;
			}
			
		}
	}

	public void Resume()
	{
		menu.SetActive(false);
		UI.SetActive(true);
		Time.timeScale = 1f;
	}

	public void Controls()
	{
		menu.SetActive(false);
		controls.SetActive(true);
	}

	public void MainMenu()
	{
		Time.timeScale = 1f;
		Level.Clear();
		SceneManager.LoadScene(0);
	}

	public void CloseControls()
	{
		menu.SetActive(true);
		controls.SetActive(false);
	}
}
