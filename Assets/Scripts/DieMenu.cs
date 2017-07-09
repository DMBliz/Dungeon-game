using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DieMenu : MonoBehaviour
{

	public void Win()
	{
		transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Congratulations you win!!";
	}

	public void SetActive()
	{
		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void MainMenu()
	{
		Level.Clear();
		SceneManager.LoadScene(0);
	}

	public void PlayAgain()
	{
		Level.Clear();

		SceneManager.LoadScene(1);
	}
}
