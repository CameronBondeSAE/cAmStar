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

	public Node current;


	public  GameObject startPrefab;
	public  GameObject targetPrefab;
	public  GameObject pathCubePrefab;
	private Vector2Int start;
	private Vector2Int target;
	public  float      visualiseSpeed = 0.1f;

	GameObject targetIndicator;
	GameObject startIndicator;

	void Start()
	{
		startIndicator  = Instantiate(startPrefab);
		targetIndicator = Instantiate(targetPrefab);

		//	    open = new List<Node>();
//	    closed = new List<Node>();
//	    finalPath = new List<Node>();

		RandomlyPositionStartAndTarget();

//		StartCoroutine(FindPath());
	}

	public void ClearMap()
	{
		finalPath.Clear();
		open.Clear();
		closed.Clear();

		foreach (Node node1 in map.grid)
		{
			if (node1.debugGO != null)
			{
				node1.Reset();
			}
		}
	}

	public void FindPathCo()
	{
		StartCoroutine(FindPath());
	}

	public void Update()
	{
		start.x = (int) startIndicator.transform.position.x;
		start.y = (int) startIndicator.transform.position.z;

		target.x = (int) targetIndicator.transform.position.x;
		target.y = (int) targetIndicator.transform.position.z;

		ClearMap();
//		RandomlyPositionStartAndTarget();
		FindPathCo();
	}

	public IEnumerator FindPath()
	{
		int  xCheck = 0;
		int  yCheck = 0;
		int  fCost;
		int  gCost;
		int  hCost;
		Node nodeToCheck;

		current = map.grid[start.x, start.y];
		open.Add(current); // Initial starting point

		// Loop until end found
		while (open.Count > 0)
		{
			current = FindLowestFCost();
			// HACK TODO DEBUG
			current.debugGO.GetComponentInChildren<Renderer>().material.color = Color.green;

			// Node is closed
			open.Remove(current);

			// TODO: Check shouldn't need the contains check
			if (!closed.Contains(current))
				closed.Add(current);


			if (CheckReachedTarget()) yield break;


			// Neighbours recalc

			for (int x = -1; x < 2; x++)
			{
				for (int y = -1; y < 2; y++)
				{
					// Same as current so bail
					if (x == 0 && y == 0)
						continue;

					xCheck = current.position.x + x;
					yCheck = current.position.y + y;

					// Bail if out of bounds or the current node, or in the closed list
					if (xCheck < 0 || yCheck < 0 || xCheck >= map.size.x || yCheck >= map.size.y)
						continue;

					nodeToCheck = map.grid[xCheck, yCheck];
					if (closed.Contains(nodeToCheck) || nodeToCheck.isBlocked)
						continue;

					// Note: Multiply by ten to maintain ints for distances
					hCost = (int) (10 * Vector2Int.Distance(
															nodeToCheck.position,
															target));
					gCost = current.gCost + (int) (10f * Vector2Int.Distance(
																			 current.position,
																			 nodeToCheck.position));

					// fCost
					fCost = hCost + gCost;

					// Bail if the existing one is lower
					if (nodeToCheck.fCost != 0 && fCost > nodeToCheck.fCost)
						continue;

					// All good, so record new values
					nodeToCheck.hCost = hCost;
					nodeToCheck.gCost = gCost;
					nodeToCheck.fCost = fCost;

					// Debug
					if (nodeToCheck.debugGO.GetComponentInChildren<TextMesh>() != null)
						nodeToCheck.debugGO.GetComponentInChildren<TextMesh>().text =
							nodeToCheck.gCost + ":" + nodeToCheck.hCost + "\n" + nodeToCheck.fCost;

					nodeToCheck.parent = current;

					Debug.DrawLine(new Vector3(nodeToCheck.position.x, 0, nodeToCheck.position.y),
								   new Vector3(nodeToCheck.position.x, 10f, nodeToCheck.position.y), Color.magenta,
								   0.1f, false);

					// TODO: Shouldn't need the contains check
					if (!open.Contains(nodeToCheck))
						open.Add(nodeToCheck);

					if (visualiseSpeed > 0) yield return new WaitForSeconds(visualiseSpeed / 10f);
				}
			}

			// HACK TODO DEBUG
			current.debugGO.GetComponentInChildren<Renderer>().material.color = Color.blue;

			if (visualiseSpeed > 0) yield return new WaitForSeconds(visualiseSpeed);
		}
	}

	private bool CheckReachedTarget()
	{
// Reached end
		if (current.position == target)
		{
//			Debug.Log("FOUND TARGET");
			while (current.parent != null)
			{
				finalPath.Add(current);
				current.debugGO.GetComponentInChildren<Renderer>().material.color = Color.green;
//				current.debugGO = Instantiate(pathCubePrefab,
//											  new Vector3(current.parent.position.x, 0, current.parent.position.y),
//											  Quaternion.identity);
				current = current.parent;
//				if (visualiseSpeed > 0) yield return new WaitForSeconds(visualiseSpeed);
			}

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


//    private void OnDrawGizmos()
//    {
//        for (int x = 0; x < map.size.x; x++)
//        {
//            for (int y = 0; y < map.size.y; y++)
//            {
//                if (map.grid != null)
//                {
//                    if (map.grid[x, y].isBlocked)
//                    {
//                        Gizmos.color = Color.red;
//                        Gizmos.DrawCube(new Vector3(x, -0.45f, y), Vector3.one);
//                    }
//
//                    if (open.Contains(map.grid[x, y]))
//                    {
//                        Gizmos.color = Color.green;
//                        Gizmos.DrawCube(new Vector3(x, -0.45f, y), Vector3.one);
//                    }
//
//                    if (closed.Contains(map.grid[x, y]))
//                    {
//                        Gizmos.color = Color.gray;
//                        Gizmos.DrawCube(new Vector3(x, -0.45f, y), Vector3.one);
//                    }
//                }
//            }
//        }


//		// Scan the real world starting at 0,0,0 (to be able to place the grid add transform.position)
//		for (int x = 0; x < size.x; x++)
//		{
//			for (int y = 0; y < size.y; y++)
//			{
//				if (Physics.CheckBox(transform.position + new Vector3(x, 0, y), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity))
//				{
//					// Something is there
//					grid[x, y].isBlocked = true;
//					Gizmos.color = Color.red;
//					Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), Vector3.one);
//				}
//			}
//		}
//    }
}