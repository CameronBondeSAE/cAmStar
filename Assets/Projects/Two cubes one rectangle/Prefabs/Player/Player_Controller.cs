using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Separate controller code TODO: Needs to use InputSystem action map events
/// Requests action on server
/// </summary>
public class Player_Controller : NetworkBehaviour
{
	public Player_Model playerModel;
	
    // Update is called once per frame
    void Update()
    {
	    // Only MY player instance should care about the key input
	    if (IsLocalPlayer)
	    {
		    if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
		    {
			    // Started holding
			    if(!IsServer)
				    playerModel.StartJump(); // Clientside prediction 'make it feel responsive'
			    RequestStartJump_Rpc(); // Actual server for real jump
		    }

		    if (InputSystem.GetDevice<Keyboard>().spaceKey.wasReleasedThisFrame)
		    {
			    // Let go of hold
			    if(!IsServer)
				    playerModel.Jump(); // Clientside prediction 'make it feel responsive'
			    RequestJump_Rpc(); // Actual server for real jump
		    }
	    }
    }
    
    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable)]
    public void RequestStartJump_Rpc()
    {
	    playerModel.StartJump();
    }
    [Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable)]
    public void RequestJump_Rpc()
    {
	    playerModel.Jump();
    }
}
