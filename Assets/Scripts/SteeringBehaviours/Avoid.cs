using System;
using System.Collections.Generic;
using UnityEngine;

public class Avoid : SteeringBehaviour_Base
{
	[SerializeField]
	private float maxDistance = 3f;

	[SerializeField]
	private Rigidbody rb;

	[SerializeField]
	public float turnSpeed = 100f;
	
	[SerializeField]
	private float turnForce = 100f;
	public  AnimationCurve curve;
	[SerializeField]
	private float curveEval;

	public List<GameObject> targets;
	
	public  int skipEveryXTimes = 2;
	private int executeCounter;
	
	private void Awake()
	{
		
	}

	private void Start()
	{
		for (int index = 0; index < targets.Count; index++)
		{
			Debug.Log(targets[index]);
		}

		foreach (GameObject item in targets)
		{
			Debug.Log(item);
		}
	}

	private void FixedUpdate()
	{
		if (executeCounter < skipEveryXTimes)
		{
			executeCounter++;
			return;
		}

		executeCounter = 0;
		
		
		
		RaycastHit hitInfo;
		bool       didItHitAnything;
		didItHitAnything = Physics.Raycast(transform.position, transform.forward, out hitInfo, maxDistance);

		if (didItHitAnything)
		{
			Debug.DrawLine(transform.position, hitInfo.point, Color.red);

			// Debug.Log(hitInfo.distance / maxDistance);
			curveEval = curve.Evaluate(hitInfo.distance / maxDistance);
			
			
			if (rb != null)
			{
				rb.AddTorque(0,turnSpeed * turnForce,0, ForceMode.Impulse);
			}
		}
		else
		{
			Debug.DrawLine(transform.position, transform.position + transform.forward * maxDistance, Color.green);
			curveEval = 1f;
		}
	}
}