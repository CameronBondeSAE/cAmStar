using System;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
	private NavMeshPath path;

	public Vector3[] Pathfind(Vector3 startPosition, Vector3 endPosition)
	{
		path = new NavMeshPath();

		if (NavMesh.CalculatePath(startPosition, endPosition, Int32.MaxValue, path))
		{
			return path.corners;
		}
		else
		{
			Debug.Log("Can't find path : "+path.status.ToString());
			return null;
		}
	}

	private void OnDrawGizmos()
	{
		if (path == null)
			return;
		
		for (int i = 0; i < path.corners.Length; i++)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(path.corners[i], 0.1f);
			if (i < path.corners.Length - 1)
			{
				Gizmos.DrawLine(path.corners[i], path.corners[i+1]);
			}
		}
	}
}
