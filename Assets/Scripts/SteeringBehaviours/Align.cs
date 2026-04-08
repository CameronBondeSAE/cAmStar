using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehaviour_Base
{
	// Variable pointing to your Neighbours component
	public  Neighbours neighbours;
	public  Rigidbody  rb;
	public  float      force = 100f;
	public  Vector3    cross;
	public  int        skipEveryXTimes = 4;
	private int        executeCounter;
	
	void FixedUpdate()
	{
		if (executeCounter < skipEveryXTimes)
		{
			executeCounter++;
			return;
		}

		executeCounter = 0;

		// Some are Torque, some are Force		
		Vector3 targetDirection = CalculateMove(neighbours.neighboursList);

		// Cross will take YOUR direction and the TARGET direction and turn 
		// it into a rotation force vector. It CROSSES through both at 90 degrees
		// Vector3 cross = Vector3.Cross(transform.forward, targetDirection);

		cross = Vector3.Cross(transform.forward, targetDirection);

		// TODO: Visualise the cross product vector/direction

		rb.AddTorque(cross * force);
	}

	public Vector3 CalculateMove(List<Transform> neighbours)
	{
		if (neighbours.Count == 0)
			return Vector3.zero;

		Vector3 alignmentDirection = Vector3.zero;

		// Average of all neighbours directions
		// I’m using a list of transforms in my neighbours script, you might be using GameObjects etc
		foreach (Transform item in neighbours)
		{
			alignmentDirection += item.forward;
		}

		alignmentDirection /= neighbours.Count;

// TODO: Draw a debug line for the DESIRED direction (Your character won’t immediately be snapping to this, line, they’re GRADUALLY turn to it)

		return alignmentDirection;
	}
}