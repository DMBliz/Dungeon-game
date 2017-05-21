using UnityEngine;
using System.Collections;

public class MoveBehaviour : MonoBehaviour
{
	const float minPathUpdateTime = 0.2f;
	const float pathUpdateMoveThreshold = 0.5f;

	public Vector2 target;
	public float speed = 0.2f;
	public float turnSpeed = 3;
	int pathIndex = 0;

	Vector2[] points;

	public void Start()
	{
		StartCoroutine(UpdatePath());
	}

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 raypos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
			RaycastHit2D hit = Physics2D.Raycast(raypos, Vector2.zero);
			if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("UnWalkable")) 
			{
				PathRequestManager.RequestPath(new PathRequest(transform.position, hit.transform.position, OnPathFound));
			}
		}
	}

	void FindPath(Vector2 targetPosition)
	{
		RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.zero);
		if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("UnWalkable"))
		{
			PathRequestManager.RequestPath(new PathRequest(transform.position, hit.transform.position, OnPathFound));
		}
	}

	void OnPathFound(Vector2[] waypoints,bool pathSucces)
	{
		if (pathSucces)
		{
			points = waypoints;
		}
	}

	IEnumerator UpdatePath()
	{
		if (Time.timeSinceLevelLoad < 0.3f)
		{
			yield return new WaitForSeconds(0.2f);
		}

		PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));

		float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;

		Vector2 targetOldPos = target;

		while (true)
		{
			yield return new WaitForSeconds(minPathUpdateTime);
			if ((target - targetOldPos).sqrMagnitude > sqrMoveThreshold)
			{
				PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
				targetOldPos = target;
			}
		}
	}

	bool FollowPath()
	{
		if (pathIndex >= points.Length || points.Length<=0)
		{
			return false;
		}

		if (new Vector2(transform.position.x, transform.position.y) == points[pathIndex])
		{
			pathIndex++;
		}
			
		transform.position = Vector2.MoveTowards(transform.position, points[pathIndex], speed*Time.deltaTime);
		return true;
	}

	float root(Vector2 lookTarget)
	{
		Vector2 dirToLook = (lookTarget - new Vector2(transform.position.x, transform.position.y)).normalized;
		float targetAngle = (Mathf.Atan2(lookTarget.y, lookTarget.x) * Mathf.Rad2Deg) - 90f;

		if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, targetAngle)) > 0.05f)
		{
			return Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.fixedDeltaTime);
		}
		return targetAngle;
	}
}
