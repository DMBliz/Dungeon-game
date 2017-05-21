using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	
	Player behaviour;
	Vector2 dir;
	// Use this for initialization
	void Start()
	{
		behaviour = GetComponent<Player>();
	}

	// Update is called once per frame
	void Update()
	{
		dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
		
	}

	void FixedUpdate()
	{
		//transform.Translate(dir * behaviour.specs.GetStat("Strength").Value * Time.deltaTime); 

		gameObject.GetComponent<Rigidbody2D>().velocity = dir * (behaviour.specs.GetStat<BaseSpec>("Strength").Value / 2f / behaviour.specs.GetStat<BaseSpec>("Strength").MaxValue ) * 100f * Time.fixedDeltaTime;
	}
}
