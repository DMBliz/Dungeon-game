using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
	public static PathRequestManager requestManager;

	Queue<PathResult> results = new Queue<PathResult>();

	PathFinder pathFinder;

	void Awake()
	{
		requestManager = this;
		pathFinder = GetComponent<PathFinder>();
	}

	void Update()
	{
		if (results.Count > 0)
		{
			int itemsCount = results.Count;
			lock (results)
			{
				for (int i = 0; i < itemsCount; i++)
				{
					PathResult res = results.Dequeue();
					res.callback(res.path, res.pathSucces);
				}
			}
		}
	}

	public static void RequestPath(PathRequest request) 
	{
		ThreadStart startThread = delegate
		{
			requestManager.pathFinder.FindPath(request, requestManager.FinishedPathFind);
		};
		startThread.Invoke();
	}

	public void FinishedPathFind(PathResult result)
	{
		lock (results)
		{
			results.Enqueue(result);
		}
	}
}

public struct PathResult
{
	public Vector2[] path;
	public bool pathSucces;
	public Action<Vector2[], bool> callback;

	public PathResult(Vector2[] path, bool succes, Action<Vector2[], bool> callback)
	{
		this.path = path;
		pathSucces = succes;
		this.callback = callback;
	}
}

public struct PathRequest
{
	public Vector2 pathStart;
	public Vector2 pathEnd;
	public Action<Vector2[], bool> callback;

	public PathRequest(Vector2 start,Vector2 end,Action<Vector2[],bool> callback)
	{
		pathStart = start;
		pathEnd = end;
		this.callback = callback;
	}
}
