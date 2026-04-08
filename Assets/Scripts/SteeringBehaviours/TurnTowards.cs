using UnityEngine;

public class TurnTowards : SteeringBehaviour_Base
{
	public Transform target;
	public Rigidbody rb;
	public float     turnSpeed = 10f;

	void FixedUpdate()
    {
	    Vector3 targetDirAndDistance = Vector3.zero;
	    
		// This gets direction AND distance
		if (target != null)
		{
			targetDirAndDistance = target.position - transform.position;
		}

		Debug.DrawRay(transform.position, targetDirAndDistance, Color.red);
	    
	    // ONLY keep direction (Unit Vector of 1 metre)
	    Vector3 direction = targetDirAndDistance.normalized;
	    
	    Debug.DrawRay(transform.position, direction, Color.green);

	    // SIGNED angle (if you don't want the sign, use Mathf.Abs() to get rid of it)
	    float angle = Vector3.SignedAngle(transform.forward, direction, transform.up);

	    // This doesn't slow down BEFORE it hits the target
	    
	    // Debug.Log((Mathf.Clamp01(angle) * 2) - 1);
	    rb.AddTorque(0, angle * turnSpeed, 0);
    }
}
