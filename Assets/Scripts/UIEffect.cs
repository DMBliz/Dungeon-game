using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIEffect : MonoBehaviour
{
	public BaseModificator modificator;
	private Image img;

	public BaseModificator Modificator
	{
		get { return modificator; }
	}

	void Awake()
	{
		img = GetComponent<Image>();
	}

	public void Init(BaseModificator modificator)
	{
		this.modificator = modificator;
		img.sprite = modificator.Icon;
	}
}
