using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlickeringLight : NetworkBehaviour
{
	[SerializeField]
	private Light flickeringLight;

	private bool currentState = false;
	
	private void FixedUpdate()
	{
		// Only happen on the server (STILL BAD CODE)
		if (IsServer)
		{
			if (Random.value > 0.995f) // Gameplay relevant
			{
				// Toggle server state data
				currentState = !currentState;

				ToggleLight_Rpc(currentState);
			}
		}
	}

	[Rpc(SendTo.ClientsAndHost)]
	private void ToggleLight_Rpc(bool state)
	{
		// Client (View)
		flickeringLight.enabled = state;
	}
}