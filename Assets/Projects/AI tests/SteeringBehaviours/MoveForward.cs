using System;
using UnityEngine;

public class MoveForward : SteeringBehaviour_Base
{
	[SerializeField]
	private Rigidbody rb;

	[SerializeField]
	private float speed = 100f;

	[SerializeField]
	float distance = 3f;

	[SerializeField]
	float slowDownForce = 250f;

	public string camsMessage = "Cam rules!";


	void FixedUpdate()
	{
		rb.AddRelativeForce(0, 0, speed);

		Ray ray = new Ray(transform.position, transform.forward);
		Physics.Raycast(ray, out RaycastHit hit, distance, Int32.MaxValue, QueryTriggerInteraction.Ignore);
		Debug.DrawRay(transform.position, transform.forward * distance, Color.cyan);
		if (hit.collider != null)
		{
			// Debug.Log(hit.collider.name);

			Debug.DrawRay(transform.position, transform.forward, Color.red);
			rb.AddRelativeForce(0, 0, -slowDownForce*(distance - hit.distance)); //-(5f-hit.distance));
		}
	}
}