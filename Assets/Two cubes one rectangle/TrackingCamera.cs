using System;
using UnityEngine;

public class TrackingCamera : MonoBehaviour
{
	public  Transform targetTransform;
	[SerializeField]
	private Vector3 offset = new Vector3(0f, 5f, -10f);

	[SerializeField]
	private float smoothSpeed = 1f;

	private void LateUpdate()
	{
		if (targetTransform != null)
		{
			transform.position = Vector3.Slerp(transform.position, targetTransform.position + offset,
			                                   Time.deltaTime * smoothSpeed);
			transform.LookAt(targetTransform);
		}
	}
}
