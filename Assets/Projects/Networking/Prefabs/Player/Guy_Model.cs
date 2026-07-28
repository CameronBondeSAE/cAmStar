using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Guy_Model : NetworkBehaviour
{
	public bool state;

	// Event for world state updates
	public delegate void LightChangeStateDelegate(bool state);
	public event LightChangeStateDelegate LightChangeState_Event;

	// Runs on the SERVER/Model
	[Rpc(SendTo.Server, Delivery = RpcDelivery.Reliable)]
	public void RequestToggleLight_Rpc()
	{
		Debug.Log("RequestToggleLight");

		state = !state;
		
		LightChangeState_Event?.Invoke(state);
	}
}
