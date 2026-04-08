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
			    RequestStartJump_Rpc();
		    }

		    if (InputSystem.GetDevice<Keyboard>().spaceKey.wasReleasedThisFrame)
		    {
			    // Let go of hold
			    RequestJump_Rpc();
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
