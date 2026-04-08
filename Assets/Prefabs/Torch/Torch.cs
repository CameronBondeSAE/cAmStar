using Unity.Netcode;
using UnityEngine;

public class Torch : NetworkBehaviour
{
	public bool  currentOnState;
	public Light light;

	void Update()
	{
		// Client Controller ONLY
		if (IsLocalPlayer)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				ActivateLight_RequestClientToServer_Rpc();
			}
		}
	}

	// Called from the Client (Controller) to ASK the server to do something for it.
	[Rpc(SendTo.Server)]
	private void ActivateLight_RequestClientToServer_Rpc()
	{
		currentOnState = !currentOnState;
		ActivateLight_Rpc(currentOnState);
	}

	// Server to clients. THEN the server syncs the VIEW to ALL CLIENTS
	[Rpc(SendTo.ClientsAndHost)]
	private void ActivateLight_Rpc(bool state)
	{
		// View
		light.enabled = state;
	}
}