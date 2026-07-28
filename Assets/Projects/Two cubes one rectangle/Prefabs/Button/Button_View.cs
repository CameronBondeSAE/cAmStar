using System;
using Unity.Netcode;
using UnityEngine;

public class Button_View : NetworkBehaviour
{
	public Button_Model buttonModel;
	public Animator animator;

	// This is running on server AND client
	// Do we want that?
	private void OnEnable()
	{
		if(IsServer)
		{
			buttonModel.Pressed_Event += Activate_Rpc;
		}
	}

	private void OnDisable()
	{
		if(IsServer)
		{
			buttonModel.Pressed_Event -= Activate_Rpc;
		}
	}

	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable)]
	private void Activate_Rpc()
	{
		Debug.Log("Activate Button");
		animator.StopPlayback();
		animator.Play("Idle");
		animator.Play("ButtonClick");
	}
}
