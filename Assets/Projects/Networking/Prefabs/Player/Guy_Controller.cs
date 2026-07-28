using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Guy_Controller : NetworkBehaviour
{
	public Guy_Model Guy_Model;
	
    // Update is called once per frame
    void Update()
    {
	    if (IsLocalPlayer)
	    {
		    if (InputSystem.GetDevice<Keyboard>().spaceKey.wasPressedThisFrame)
		    {
			    Debug.Log("Client: Try space: ID = "+GetComponent<NetworkObject>().NetworkObjectId);
			    
			    Guy_Model.RequestToggleLight_Rpc();
		    }
	    }
    }
}
