using UnityEngine;

public class PlayerController : MonoBehaviour
{
	Vector2 dir;

	[SerializeField]
	private float speed = 5f;

	private Rigidbody2D rigidbody2d;
	private float increaseSpeed = 0f;
	[SerializeField]
	private float maxIncrease = 10f;
	[SerializeField]
	private float coolDownTime = 2f;
	private float coolDown = 0f;
	[SerializeField]
	private float turnSpeed = 120f;
	[SerializeField]
	private Vector2 targetVector;

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

		targetVector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition) - transform.position;
		float targetAngle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg - 90;
		float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
		transform.eulerAngles = Vector3.forward * angle;
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

	void OnDrawGizmos()
	{
		Gizmos.DrawRay(transform.position, transform.up*10);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, targetVector);
	}
}
