using Unity.Netcode;
using UnityEngine;

public class Torch_Controller : NetworkBehaviour
{
	public Torch_Model torchModel;
	
	void Update()
	{
		// Client Controller ONLY
		if (IsLocalPlayer)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("Controller toggle light : isClient = "+IsClient + " : isServer = "+IsServer);
				torchModel.ActivateLight_RequestClientToServer_Rpc();
			}
		}
	}
}