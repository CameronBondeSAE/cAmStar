using UnityEngine;

public class TurnTowards : SteeringBehaviour_Base
{
	public Transform      target;
	public Rigidbody      rb;
	public float          turnSpeed = 10f;
	public AnimationCurve turnSpeedCurve;

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
	    // rb.AddTorque(0, Mathf.Clamp(angle, -1f, 1f) * turnSpeed, 0);
	 
	    // To use curves, you need a 0 to X value. It's awkward to have negative values, so I ABS my angle
	    float angleWithNoSign  = Mathf.Abs(angle);
		// I'll need the direction back though, so I'll multiply the result with this -1 to 1 value.
		float positiveNegative = Mathf.Clamp(angle, -1f, 1f);
	    rb.AddTorque(0, turnSpeedCurve.Evaluate(angleWithNoSign/180f) * turnSpeed * positiveNegative, 0);
	    // rb.AddTorque(0, Mathf.Clamp(angle, -10f, 10f) * turnSpeed, 0);
	    // rb.AddTorque(0, angle * turnSpeed, 0);
    }
}
