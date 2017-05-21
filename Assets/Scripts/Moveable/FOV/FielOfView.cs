using UnityEngine;
using System;
using System.Threading;
using System.Collections;

[Serializable]
public class FielOfView
{
	public event Action OnFindPlayer;
	public event Action OnLosePlayer;
	public PawnBehaviour owner;
	[SerializeField]
	float Radius = 5f;
	[SerializeField]
	[Range(0, 360)]
	public float Angle = 90f;
	public GameObject player;
	bool bIsSeePlayer = false;

	public FielOfView(PawnBehaviour owner)
	{
		this.owner = owner;
		player = GameObject.FindGameObjectWithTag("Player");
	}
	

	public IEnumerator Find()
	{
		while (true)
		{
			FindVisible();
			yield return new WaitForSeconds(0.2f);
		}
	}

	void FindVisible()
	{
		if (player != null && Vector2.Distance(owner.transform.position, player.transform.position) < Radius)
		{
			if (Vector2.Angle(owner.transform.up, (player.transform.position - owner.transform.position).normalized) < Angle / 2)
			{
				RaycastHit2D hit = Physics2D.Linecast(owner.transform.position, player.transform.position, WorldManager.worldManager.unWalkable);
				if (hit.collider == null) 
				{
					if (!bIsSeePlayer)
					{
						bIsSeePlayer = true;
						if (OnFindPlayer != null)
							OnFindPlayer();
					}
				}
				else if (hit.collider != null && bIsSeePlayer) 
				{
					if (OnLosePlayer != null)
						OnLosePlayer();
				
					bIsSeePlayer = false;
				}
			}
			else if (bIsSeePlayer)
			{
				if (OnLosePlayer != null)
					OnLosePlayer();

				bIsSeePlayer = false;
			}
		}
		else if(bIsSeePlayer)
		{			
			if (OnLosePlayer != null)
				OnLosePlayer();
			
			bIsSeePlayer = false;
		}
	}

	public Vector2 DirFromAngle(float angleDeg)
	{
		angleDeg += owner.transform.eulerAngles.z + 90f;

		return new Vector2(Mathf.Cos(angleDeg * Mathf.Deg2Rad), Mathf.Sin(angleDeg * Mathf.Deg2Rad));
		
	}

	public float AngleFromDir(Vector2 dir)
	{
		return (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) - 90f;
	}

	public void OnDrawGizmos()
	{
		if (owner != null)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireSphere(owner.transform.position, Radius);

			Vector2 pointA = DirFromAngle(-Angle / 2);
			Vector2 pointB = DirFromAngle(Angle / 2);
			Gizmos.DrawLine(owner.transform.position, new Vector2(owner.transform.position.x, owner.transform.position.y) + pointA * Radius);
			Gizmos.DrawLine(owner.transform.position, new Vector2(owner.transform.position.x, owner.transform.position.y) + pointB * Radius);
			Gizmos.color = Color.red;
			Gizmos.DrawRay(owner.transform.position, owner.transform.up*Radius);
		}
	}
}
