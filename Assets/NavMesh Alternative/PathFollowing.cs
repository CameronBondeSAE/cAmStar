using System;
using UnityEngine;

public class PathFollowing : MonoBehaviour
{
	public Pathfinding pathfinding;
	public Transform   feet;
	public Transform   target;

	public int   currentNodeIndex;
	public float speed;
	public float distanceToNextNodeThreshold;

	public Vector3[]    finalPathArray;

	Vector3 nextNodePos;
	
	private void Start()
	{
		finalPathArray = pathfinding.Pathfind(feet.position, target.position);
	}

	private void FixedUpdate()
    {
		// No path to target. Probably blocked
	    if (finalPathArray == null || finalPathArray.Length <= 0)
	    {
		    return;
	    }
			
	    nextNodePos = finalPathArray[currentNodeIndex];

	    // TERRIBLE move to next node. DON'T USE THIS. Use actual steering behaviours script.
	    transform.LookAt(new Vector3(nextNodePos.x, transform.position.y, nextNodePos.z));
	    transform.Translate(0,0,speed*Time.fixedDeltaTime, Space.Self);

	    // Check next nearest node in path
	    if (Vector3.Distance(transform.position, nextNodePos) < distanceToNextNodeThreshold)
	    {
		    currentNodeIndex++;

		    // At target
		    if (currentNodeIndex > finalPathArray.Length-1)
		    {
			    finalPathArray   = null;
			    currentNodeIndex = 0;
		    }
	    }
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(nextNodePos, 1f);
	}
}
