using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGuy : MonoBehaviour
{
	public float speed = 1f;

	public enum States
	{
		Idle,
		GoTo
	}

	public States state;
	
	Map     map;
	cAmStar AStar;


	// Start is called before the first frame update
	void Start()
	{
		// TODO: HACK probably should be a singleton or something
		map     = FindObjectOfType<Map>();
		AStar = FindObjectOfType<cAmStar>();
	}

	public List<Node> finalPath;
	public Node[] finalPathArray;
	private void WanderAround()
	{
		AStar.OnBlockedPath -= OnBlockedPath;
		AStar.OnFoundPath   -= OnFoundPath;

		AStar.OnBlockedPath += OnBlockedPath;
		AStar.OnFoundPath   += OnFoundPath;

		
		// Coroutine visualise
		AStar.FindPath(new Vector2(transform.position.x, transform.position.z), map.FindUnblockedSpace());

		// Instant
		// finalPath = AStar.FindPath(new Vector2(transform.position.x, transform.position.z), map.FindUnblockedSpace());
		// finalPathArray = new Node[finalPath.Count];
		// finalPath.CopyTo(finalPathArray);
	}

	private void OnFoundPath()
	{
		// Visualise speed coroutine
		finalPath = AStar.finalPath;
		finalPathArray = new Node[finalPath.Count];
		finalPath.CopyTo(finalPathArray);

		state = States.GoTo;
	}

	private void OnBlockedPath()
	{
		state = States.Idle;
	}

	private int currentNodeIndex = 0;
	public float distanceToNextNodeThreshold;

	// Update is called once per frame
	void FixedUpdate()
	{
		if (state == States.Idle)
		{
			WanderAround();
			state = States.GoTo;
		}
		
		if (state == States.GoTo)
		{
			// No path to target. Probably blocked
			if (finalPathArray == null || finalPathArray.Length <= 0)
			{
				// state = States.Idle;
				return;
			}
			
			Vector2 nextNodePos = finalPathArray[currentNodeIndex].position;

			// Move to next node
			transform.LookAt(new Vector3(nextNodePos.x, transform.position.y, nextNodePos.y));
			transform.Translate(0,0,speed, Space.Self);

			// Check next nearest node in path
			if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), nextNodePos) < distanceToNextNodeThreshold)
			{
				currentNodeIndex++;

				// At target
				if (currentNodeIndex > finalPathArray.Length-1)
				{
					finalPathArray = null;
					state = States.Idle;
					currentNodeIndex = 0;
				}
			}
		}
	}
}