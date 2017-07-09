using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject menu;
	[SerializeField]
	private GameObject controls;

	public void Play()
	{
		SceneManager.LoadScene(1);
	}

	public void Controls()
	{
		controls.SetActive(true);
		menu.SetActive(false);
	}

	public void HideControls()
	{
		controls.SetActive(false);
		menu.SetActive(true);
	}

	public void Exit()
	{
		Application.Quit();
	}
}
