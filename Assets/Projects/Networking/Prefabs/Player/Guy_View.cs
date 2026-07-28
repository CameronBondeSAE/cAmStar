using System;
using Unity.Netcode;
using UnityEngine;

public class Guy_View : NetworkBehaviour
{
	public Light light;
	public Guy_Model Guy_Model;

	private void OnEnable()
	{
		Guy_Model.LightChangeState_Event += ChangeLightState_Rpc;
	}

	private void OnDisable()
	{
		Guy_Model.LightChangeState_Event -= ChangeLightState_Rpc;
	}

	// View/Client
	// Just display of model data
	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable)]
	private void ChangeLightState_Rpc(bool newState)
	{
		if (IsClient)
		{
			light.enabled = newState; // Client/View
		}
	}
}
