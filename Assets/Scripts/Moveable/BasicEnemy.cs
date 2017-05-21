using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : PawnBehaviour
{
	[Header("Behaviour")]

	[SerializeField]
	EEnemyState state = EEnemyState.None;

	[SerializeField]
	List<Vector2> patrolPoints = new List<Vector2>();
	[SerializeField]
	int curentPointIndex;

	[SerializeField]
	FielOfView fov;

	[Header("Move")]
	const float minPathUpdateTime = 0.2f;
	const float pathUpdateMoveThreshold = 0.5f;

	[SerializeField]
	Vector2[] points;

	[SerializeField]
	int pathIndex = 0;

	[SerializeField]
	public Vector2 target;

	[SerializeField]
	Vector2 lastPosition = new Vector2();

	[SerializeField]
	public float speed = 2f;

	[SerializeField]
	float turnSpeed = 40f;

	new void Awake()
	{
		base.Awake();
		curentPointIndex = 0;
		EnemyTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		state = EEnemyState.Patrol;
		fov.owner = this;
		fov.OnFindPlayer += PlayerFind;
		fov.OnLosePlayer += PlayerLose;
		fov.player = EnemyTarget.gameObject;
		StartCoroutine(fov.Find());
	}

	void PlayerFind()
	{
		state = EEnemyState.Attack;
	}

	void PlayerLose()
	{
		state = EEnemyState.Search;
		lastPosition = EnemyTarget.transform.position;
	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		switch (state)
		{
			case EEnemyState.None:
				break;
			case EEnemyState.Patrol:
				Patrol();
				break;
			case EEnemyState.Attack:
				Attack();
				break;
			case EEnemyState.Search:
				Search();
				break;
			case EEnemyState.Wait:
				Wait();
				break;
			default:
				break;
		}
	}

	bool Move(Vector2 moveTarget, bool withRoot=false)
	{
		if (moveTarget != target)
		{
			target = moveTarget;
			FindPath(target);
			return true;
		}
		if (points != null)
		{
			return FollowPath(withRoot);
		}
		else
		{
			return true;
		}

	}

	void FindPath(Vector2 targetPosition)
	{
		RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.zero);
		if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("UnWalkable"))
		{
			PathRequestManager.RequestPath(new PathRequest(transform.position, targetPosition, OnPathFound));
		}
	}

	void OnPathFound(Vector2[] waypoints, bool pathSucces)
	{
		if (pathSucces)
		{
			points = waypoints;
		}
		else
		{
			points = new Vector2[0];
		}
	}

	bool FollowPath(bool withRoot)
	{
		if (points == null)
		{
			return true;
		}

		if (pathIndex >= points.Length || points.Length <= 0)
		{
			points = null;
			pathIndex = 0;
			return false;
		}

		if (withRoot)
		{
			if (Root(points[pathIndex]))
			{
				return true;
			}
		}

		transform.position = Vector2.MoveTowards(transform.position, points[pathIndex], speed * Time.deltaTime);

		if (new Vector2(transform.position.x, transform.position.y) == points[pathIndex])
		{
			pathIndex++;
		}

		return true;
	}

	bool Root(Vector2 lookTarget)
	{
		Vector2 dirToLook = (lookTarget - new Vector2(transform.position.x, transform.position.y)).normalized;
		float targetAngle = fov.AngleFromDir(dirToLook);

		if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.05f)
		{
			float rootAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.fixedDeltaTime);
			transform.eulerAngles = Vector3.forward * rootAngle;
			return true;
		}
		return false;
	}

	void Look(Vector2 lookTarget)
	{
		Vector2 dirToLook = (lookTarget - new Vector2(transform.position.x, transform.position.y)).normalized;
		float targetAngle = fov.AngleFromDir(dirToLook);
		transform.eulerAngles = Vector3.forward * targetAngle;
	}

	void Patrol()
	{
		if(!Move(patrolPoints[curentPointIndex],true))
		{
			curentPointIndex = (curentPointIndex + 1) % patrolPoints.Count;
			state = EEnemyState.Wait;
		}

	}

	Vector2 UpDirection;
	Vector2[] rootDirections;
	int currentDirection = 0;

	void Wait()
	{
		if (UpDirection == Vector2.zero)
		{
			UpDirection = transform.up;
			currentDirection = 0;
			rootDirections = new Vector2[UnityEngine.Random.Range(2, 5)];

			for (int i = 0; i < rootDirections.Length; i++)
			{
				float rootAngle = UnityEngine.Random.Range(1, 3) % 2 == 1 ? UnityEngine.Random.Range(-120, -60) : UnityEngine.Random.Range(60, 120);
				float sinOfAngle = Mathf.Sin(rootAngle);
				float cosOfAngle = Mathf.Cos(rootAngle);
				Vector2 rootVec = new Vector2(UpDirection.x * sinOfAngle - UpDirection.y * cosOfAngle, UpDirection.x * cosOfAngle + UpDirection.y * sinOfAngle);
				rootDirections[i] = rootVec;
			}
		}

		if (currentDirection < rootDirections.Length)
		{
			if (!Root(rootDirections[currentDirection]))
			{
				currentDirection++;
			}
		}
		else
		{
			UpDirection = Vector2.zero;
			rootDirections = null;
			currentDirection = 0;
			state = EEnemyState.Patrol;
		}
	}

	void Attack()
	{
		if (Vector2.Distance(new Vector2(EnemyTarget.transform.position.x, EnemyTarget.transform.position.y), new Vector2(transform.position.x, transform.position.y)) < EquipedWeapon.weaponStats.RangeOfAttack) 
		{
			Attack(EnemyTarget);
		}
		else
		{
			Move(EnemyTarget.transform.position);
			Look(EnemyTarget.transform.position);
		}
	}

	void Search()
	{
		if (!Move(lastPosition))
		{
			state = EEnemyState.Wait;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		fov.OnDrawGizmos();
		for (int i = 0; i < patrolPoints.Count; i++)
		{
			Gizmos.DrawSphere(patrolPoints[i], 0.1f);
		}
	}
}

public enum EEnemyState
{
	None,
	Patrol,
	Attack,
	Search,
	Wait
}