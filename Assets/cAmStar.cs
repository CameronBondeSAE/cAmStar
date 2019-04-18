using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cAmStar : MonoBehaviour
{
	public Map map;

	public List<Node> open;
	public List<Node> closed;
	public List<Node> finalPath;

	public Node parent;


	public GameObject startPrefab;
	public GameObject targetPrefab;
	public GameObject pathCubePrefab;
	public Vector2    start;
	public Vector2    target;
	public float      visualiseSpeed = 0.1f;

	GameObject targetIndicator;
	GameObject startIndicator;

	public event Action OnFoundPath;
	public event Action OnBlockedPath;

	void Start()
	{
		startIndicator  = Instantiate(startPrefab);
		targetIndicator = Instantiate(targetPrefab);

		RandomlyPositionStartAndTarget();

//        DemoMode();
//        OnFoundPath += DemoMode;
	}

	public void DemoMode()
	{
		RandomlyPositionStartAndTarget();
		FindPath();
	}

	public void ClearMap()
	{
		finalPath.Clear();
		open.Clear();
		closed.Clear();

		foreach (Node node in map.grid)
		{
			if (node.debugGO != null)
			{
				node.Reset();
			}
		}
	}

	public List<Node> FindPath()
	{
//        StartCoroutine(FindPathCoroutine());
		return FindPathCoroutine();
	}

	public List<Node> FindPath(Vector2 _start, Vector2 _target)
	{
		start  = _start;
		target = _target;

		startIndicator.transform.position  = new Vector3(start.x, 0, start.y);
		targetIndicator.transform.position = new Vector3(target.x, 0, target.y);

		return FindPath();
	}

//	private IEnumerator FindPathCoroutine()
	private List<Node> FindPathCoroutine()
	{
		// Debug
		ClearMap();

		float xCheck = 0;
		float yCheck = 0;
		int   fCost;
		int   gCost;
		int   hCost;
		Node  neighbour;

		parent = map.grid[(int) start.x, (int) start.y];
		open.Add(parent); // Initial starting point

		// Loop until end found
		while (open.Count > 0)
		{
			parent = FindLowestFCost();
			// HACK TODO DEBUG
//			parent.debugGO.GetComponentInChildren<Renderer>().material.color = Color.green;

			// Node is closed
			open.Remove(parent);

			// TODO: Check shouldn't need the contains check
			if (!closed.Contains(parent))
				closed.Add(parent);


			if (CheckReachedTarget())
			{
//                yield return new WaitForSeconds(2f);
				OnFoundPath?.Invoke();
				
				//                yield break;
				return finalPath;
			}


			// Neighbours recalc
			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					// Same as current so bail
					if (x == 0 && y == 0)
						continue;

					xCheck = parent.position.x + x;
					yCheck = parent.position.y + y;

					// Bail if out of bounds or the current node, or in the closed list
					if (xCheck < 0 || yCheck < 0 || xCheck >= map.size.x || yCheck >= map.size.y)
						continue;

					neighbour = map.grid[(int) xCheck, (int) yCheck];
					// Bail if node used or blocked
					if (closed.Contains(neighbour) || neighbour.isBlocked)
						continue;

					// Note: Multiply by ten to maintain ints for distances
					hCost = (int) (10 * Vector2.Distance(
														 neighbour.position,
														 target));
					gCost = parent.gCost + (int) (10f * Vector2.Distance(
																		  parent.position,
																		  neighbour.position));

					// fCost
					fCost = hCost + gCost;

					// Bail if the existing fCost is lower
					// HACK, don't check for 0, at least do -1 ffs
					if (neighbour.fCost != 0 && fCost > neighbour.fCost)
						continue;

					// All good, so record new values (don't do it WHILE you're calculating the f,g,h costs because they rely on previous results)
					neighbour.hCost = hCost;
					neighbour.gCost = gCost;
					neighbour.fCost = fCost;

					// Debug
//					if (neighbour.debugGO.GetComponentInChildren<TextMesh>() != null)
//						neighbour.debugGO.GetComponentInChildren<TextMesh>().text =
//							neighbour.gCost + ":" + neighbour.hCost + "\n" + neighbour.fCost;

					neighbour.parent = parent;

//					Debug.DrawLine(new Vector3(neighbour.position.x, 0, neighbour.position.y),
//								   new Vector3(neighbour.position.x, 10f, neighbour.position.y), Color.magenta,
//								   0.1f, false);

					// TODO: Shouldn't need the contains check
					if (!open.Contains(neighbour))
						open.Add(neighbour);

//                    if (visualiseSpeed > 0) yield return new WaitForSeconds(visualiseSpeed / 10f);
				}
			}

			// HACK TODO DEBUG
//			parent.debugGO.GetComponentInChildren<Renderer>().material.color = Color.blue;

//            if (visualiseSpeed > 0) yield return new WaitForSeconds(visualiseSpeed);
		}

//        yield return new WaitForSeconds(2f);
		OnBlockedPath?.Invoke();
		return null;
	}

	private bool CheckReachedTarget()
	{
		// Reached end
		if (parent.position == target)
		{
			while (parent.parent != null)
			{
				finalPath.Add(parent);
				parent.debugGO.GetComponentInChildren<Renderer>().material.color = Color.green;
				parent                                                           = parent.parent;
//				if (visualiseSpeed > 0) yield return new WaitForSeconds(visualiseSpeed);
			}

			// Because it get added from the END back to the start
			finalPath.Reverse();

			return true;
		}

		return false;
	}

	private Node FindLowestFCost()
	{
//        int lowest = open.Min(Node => Node.fCost);

		// Find next lowest fCost
		int  lowestFCost     = int.MaxValue;
		Node lowestFCostNode = null;

		foreach (Node node in open)
		{
			if (node.fCost < lowestFCost)
			{
				lowestFCost     = node.fCost;
				lowestFCostNode = node;
			}
		}

		return lowestFCostNode;
	}

	public void RandomlyPositionStartAndTarget()
	{
		start                              = map.FindUnblockedSpace();
		startIndicator.transform.position  = new Vector3(start.x, 0, start.y);
		target                             = map.FindUnblockedSpace();
		targetIndicator.transform.position = new Vector3(target.x, 0, target.y);
	}


//    public void Update()
//    {
//        // Don't continuously update if we want to visualise manually
//        if (visualiseSpeed > 0)
//            return;
//
//        start.x = (int) startIndicator.transform.position.x;
//        start.y = (int) startIndicator.transform.position.z;
//
//        target.x = (int) targetIndicator.transform.position.x;
//        target.y = (int) targetIndicator.transform.position.z;
//
//        ClearMap();
////		RandomlyPositionStartAndTarget();
//        FindPath();
//    }


    private void OnDrawGizmos()
    {
	    Vector3 size = new Vector3(1,0.1f,1);
	    
	    Gizmos.color = Color.yellow;
	    foreach (Node node in open)
	    {
		    Gizmos.DrawCube( new Vector3(node.position.x, 0, node.position.y), size);
	    }

	    Gizmos.color = Color.black;
	    foreach (Node node in closed)
	    {
		    Gizmos.DrawCube( new Vector3(node.position.x, 0, node.position.y), size);
	    }

	    Gizmos.color = Color.green;
	    foreach (Node node in finalPath)
	    {
		    Gizmos.DrawCube( new Vector3(node.position.x, 0, node.position.y), size);
	    }
    }
}