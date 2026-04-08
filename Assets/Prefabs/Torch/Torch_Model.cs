using System;
using Unity.Netcode;

public class Torch_Model : NetworkBehaviour
{
	public NetworkVariable<bool> currentOnState;

	// Called from the Client (Controller) to ASK the server to do something for it.
	[Rpc(SendTo.Server)]
	public void ActivateLight_RequestClientToServer_Rpc()
	{
		if (IsServer)
		{
			// The ACTUAL state of the game. What it looks doesn't matter at this stage
			currentOnState.Value = !currentOnState.Value;
		}
	}
}