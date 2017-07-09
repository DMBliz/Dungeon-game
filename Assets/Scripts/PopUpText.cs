using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
	private Transform target;

	public Transform Target
	{
		get { return target; }
		set { target = value; }
	}

	void Start ()
	{
		Destroy(gameObject, transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
	}

	void Update()
	{
		if (target != null)
		{
			transform.position = Camera.main.WorldToScreenPoint(target.position);
		}
	}
}
