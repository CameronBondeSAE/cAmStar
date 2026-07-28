using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkPositionRotationPhysics : NetworkBehaviour
{
	public  Transform   myTransform;
	public  Rigidbody2D myRigidbody2D;
	[SerializeField]
	private float updateDelay = 1f;

	private void Start()
	{
		if(IsServer)
			StartCoroutine(LoopForUpdates());
	}

	public void ForceUpdatePositionRotationPhysics()
	{
		UpdatePositionRotationPhysics_Rpc(transform.position, transform.rotation, myRigidbody2D.linearVelocity, myRigidbody2D.angularVelocity);
	}
	
	private IEnumerator LoopForUpdates()
	{
		while (true)
		{
			UpdatePositionRotationPhysics_Rpc(transform.position, transform.rotation, myRigidbody2D.linearVelocity, myRigidbody2D.angularVelocity);
			yield return new WaitForSeconds(updateDelay);
		}
	}

	// Runs on the clients ghosts
	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Unreliable, InvokePermission = RpcInvokePermission.Everyone)]
	private void UpdatePositionRotationPhysics_Rpc(Vector3 transformPosition, Quaternion transformRotation, Vector2 linearVelocity, float angularVelocity)
	{
		myTransform.position = transformPosition;
		myTransform.rotation = transformRotation;
		myRigidbody2D.linearVelocity = linearVelocity;
		myRigidbody2D.angularVelocity = angularVelocity;
	}
}
