using UnityEngine;
using System.Collections.Generic;

public class UIModificatorPanel : MonoBehaviour
{
	[SerializeField]
	private List<UIEffect> modificators = new List<UIEffect>();
	[SerializeField]
	private GameObject modificatorImage;

	public void AddModificator(BaseModificator modificator)
	{
		GameObject tmp = Instantiate(modificatorImage);
		tmp.transform.SetParent(transform);
		tmp.GetComponent<UIEffect>().Init(modificator);
		modificators.Add(tmp.GetComponent<UIEffect>());
	}

	public void RemoveModificator(BaseModificator modificator)
	{
		UIEffect effect = modificators.Find(x => x.GetComponent<UIEffect>().modificator == modificator);
		if (effect != null)
		{
			modificators.Remove(effect);
			Destroy(effect.gameObject);
		}
	}
}
