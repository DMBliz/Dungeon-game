using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public static CameraManager instance;
	public GameObject target;

	public GameObject Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
			transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
			
		}
	}

	public float range;
	public float speed;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		if (target != null)
		{
			transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
		}
	}

	void Update ()
	{
		if (target != null)
		{
			if (Vector2.Distance(transform.position, target.transform.position) > range)
			{
				transform.position = Vector2.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
				transform.position = new Vector3(transform.position.x, transform.position.y, -10);
			}
		}
	}
}
