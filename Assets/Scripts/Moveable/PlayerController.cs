using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Vector2 dir;

	[SerializeField]
	private float speed = 5f;

	private Rigidbody2D rigidbody2d;
	private float increaseSpeed = 0f;
	private float maxIncrease = 10f;
	private float coolDownTime = 5f;
	private float coolDown = 0f;

	void Start()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
		if (Input.GetKeyDown(KeyCode.Space) && coolDown <= 0f && dir.magnitude > 0)
		{
			increaseSpeed = maxIncrease;
			coolDown = coolDownTime;
		}
		if (coolDown > 0)
		{
			coolDown -= Time.deltaTime;
		}
	}

	void FixedUpdate()
	{
		//transform.Translate(dir * behaviour.specs.GetStat("Strength").Value * Time.deltaTime); 
		
		if (increaseSpeed > 0)
		{
			rigidbody2d.velocity = dir * speed * increaseSpeed * Time.fixedDeltaTime;
			increaseSpeed -= maxIncrease * 0.15f;
		}
		else
		{
			rigidbody2d.velocity = dir * speed * Time.fixedDeltaTime;
		}
	}
}
