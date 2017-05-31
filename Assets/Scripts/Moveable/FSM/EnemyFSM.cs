using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class EnemyFSM
{
	private FSM fsm;

	[SerializeField] private float turnSpeed;

	[SerializeField]
	private float speed = 2f;

	[SerializeField]
	Vector2[] points;

	private Vector2 lastPosition;
	int pathIndex = 0;

	[SerializeField]
	public List<Vector2> patrolPoints = new List<Vector2>();
	private int curentPatrolIndex;
	[SerializeField] private bool drawPatroolPoints = false;

	[SerializeField]
	private FielOfView fov;

	private PawnBehaviour owner;

	private float attackDistance = 1f;

	public float AttackDistance
	{
		get { return attackDistance; }
		set { attackDistance = value; }
	}

	private PawnBehaviour player;
	[SerializeField] private bool drawPath = false;

	public EnemyFSM()
	{
	}

	public EnemyFSM(FSM fsm, PawnBehaviour owner)
	{
		this.fsm = fsm;
		fsm.PushState(Patrol);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PawnBehaviour>();
		this.owner = owner;
		fov = new FielOfView(owner);
		fov.OnFindPlayer += FindPlayer;
		fov.OnLosePlayer += LosePlayer;
		curentPatrolIndex = 0;
		fsm.StartCoroutine(fov.Find());

		patrolPoints.Add(new Vector2(-17,7));
		patrolPoints.Add(new Vector2(5, 18));
		patrolPoints.Add(new Vector2(16, -10));
		patrolPoints.Add(new Vector2(-15, -11));
	}

	public void Init(FSM fsm, PawnBehaviour owner)
	{
		this.fsm = fsm;
		fsm.PushState(Patrol);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PawnBehaviour>();
		this.owner = owner;
		fov.Init(owner);
		fov.OnFindPlayer += FindPlayer;
		fov.OnLosePlayer += LosePlayer;
		curentPatrolIndex = 0;
	}

	void FindPlayer()
	{
		if(fsm.GetCurrentState() == FindTarget)
			fsm.PopState();

		fsm.PushState(Attack);
	}

	void LosePlayer()
	{
		if (fsm.GetCurrentState() == Attack)
			fsm.PopState();

		fsm.PushState(FindTarget);
		lastPosition = player.transform.position;
	}

	void Attack()
	{
		if (!MoveToPlayer(attackDistance))
		{
			if(!player.IsDead)
				owner.Attack(player.GetComponent<PawnBehaviour>());
			else
			{
				fsm.PopState();
				fsm.PushState(Patrol);
			}
		}
	}

	void Patrol()
	{
		if (!Move(patrolPoints[curentPatrolIndex]))
		{
			curentPatrolIndex = (curentPatrolIndex + 1) % patrolPoints.Count;
			fsm.PushState(LookAround);
		}
	}

	private Vector2 UpDirection = Vector2.zero;
	Vector2[] rootDirections;
	int currentDirection = 0;

	void LookAround()
	{
		if (UpDirection == Vector2.zero)
		{
			UpDirection = fsm.transform.up;
			currentDirection = 0;
			rootDirections = new Vector2[UnityEngine.Random.Range(2, 5)];

			float rootAngle = UnityEngine.Random.Range(1, 3) % 2 == 1 ? UnityEngine.Random.Range(-120, -60) : UnityEngine.Random.Range(60, 120);
			float sinOfAngle = Mathf.Sin(rootAngle);
			float cosOfAngle = Mathf.Cos(rootAngle);
			Vector2 rootVec = new Vector2(UpDirection.x * sinOfAngle - UpDirection.y * cosOfAngle, UpDirection.x * cosOfAngle + UpDirection.y * sinOfAngle);

			rootDirections[0] = rootVec;

			for (int i = 1; i < rootDirections.Length; i++)
			{
				rootAngle = UnityEngine.Random.Range(1, 3) % 2 == 1 ? UnityEngine.Random.Range(-120, -60) : UnityEngine.Random.Range(60, 120);
				sinOfAngle = Mathf.Sin(rootAngle);
				cosOfAngle = Mathf.Cos(rootAngle);
				rootVec = new Vector2(rootDirections[i - 1].x * sinOfAngle - rootDirections[i - 1].y * cosOfAngle, rootDirections[i - 1].x * cosOfAngle + rootDirections[i - 1].y * sinOfAngle);
				rootDirections[i] = rootVec;
			}
		}

		if (currentDirection < rootDirections.Length)
		{
			if (!Rotate(rootDirections[currentDirection], turnSpeed / 2f))
			{
				currentDirection++;
			}
		}
		else
		{
			UpDirection = Vector2.zero;
			rootDirections = null;
			currentDirection = 0;
			fsm.PopState();
		}
	}

	void FindTarget()
	{
		if (!Move(lastPosition))
		{
			fsm.PopState();
			fsm.PushState(LookAround);
		}
	}

	bool MoveToPlayer(float distance)
	{
		Rotate(player.transform.position, turnSpeed * 2f);
		if (Vector2.Distance(owner.transform.position, player.transform.position) > distance)
		{
			owner.transform.position = Vector2.MoveTowards(owner.transform.position, player.transform.position, speed * Time.deltaTime);
			return true;
		}
		return false;
	}

	bool Move(Vector2 target)
	{
		if (target == new Vector2(owner.transform.position.x, owner.transform.position.y))
		{
			inSearchingPath = false;
			findPath = false;
			return false;
		}

		if (!inSearchingPath && !findPath)
		{
			FindPath(target);
			return true;
		}

		if (inSearchingPath && !findPath)
		{
			return true;
		}

		if (findPath && points != null && points.Length > 0)
		{
			if (!FollowPath(true))
			{
				inSearchingPath = false;
				findPath = false;
				return false;
			}
			return true;
		}
		inSearchingPath = false;
		findPath = false;
		return false;
	}
	
	bool FollowPath(bool withRoot)
	{
		if (pathIndex >= points.Length || points.Length <= 0)
		{
			points = null;
			pathIndex = 0;
			return false;
		}

		if (withRoot)
		{
			if (Rotate(points[pathIndex], turnSpeed))
			{
				return true;
			}
		}

		owner.transform.position = Vector2.MoveTowards(fsm.transform.position, points[pathIndex], speed * Time.deltaTime);

		if (new Vector2(fsm.transform.position.x, owner.transform.position.y) == points[pathIndex])
		{
			pathIndex++;
		}

		return true;
	}

	private bool inSearchingPath = false;
	private bool findPath = false;

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

		findPath = true;
		inSearchingPath = false;
	}

	void FindPath(Vector2 targetPosition)
	{
		RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.zero);
		if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("UnWalkable"))
		{
			inSearchingPath = true;
			PathRequestManager.RequestPath(new PathRequest(fsm.transform.position, targetPosition, OnPathFound));
		}
	}

	bool Rotate(Vector2 target, float speed)
	{
		Vector2 dirToLook = (target - new Vector2(owner.transform.position.x, owner.transform.position.y)).normalized;
		float targetAngle = fov.AngleFromDir(dirToLook);

		if (Mathf.Abs(Mathf.DeltaAngle(owner.transform.eulerAngles.z, targetAngle)) > 1.1f)
		{
			float rootAngle = Mathf.MoveTowardsAngle(owner.transform.eulerAngles.z, targetAngle, speed * Time.fixedDeltaTime);
			owner.transform.eulerAngles = Vector3.forward * rootAngle;
			return true;
		}
		return false;
	}

	public void DrawGizmos()
	{
		Gizmos.color = Color.red;
		fov.OnDrawGizmos();
		if (drawPath)
		{
			if (points != null)
			{
				foreach (Vector2 point in points)
				{
					Gizmos.DrawCube(point, Vector3.one * 0.5f);
				}
			}
		}
		Gizmos.color = Color.cyan;
		if (drawPatroolPoints)
		{
			if (patrolPoints != null)
			{
				foreach (Vector2 point in patrolPoints)
				{
					Gizmos.DrawCube(point, Vector3.one * 0.5f);
				}
			}
		}
		fov.OnDrawGizmos();
	}
}
