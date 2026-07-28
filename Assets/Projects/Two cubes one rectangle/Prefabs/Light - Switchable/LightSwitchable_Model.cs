using Unity.Netcode;
using UnityEngine;

public struct LightChangePacket : INetworkSerializable
{
	public bool  newState;
	public Color colour;
	public float intensity;
	
	public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
	{
		serializer.SerializeValue(ref newState);
		serializer.SerializeValue(ref colour);
		serializer.SerializeValue(ref intensity);
	}
}

public class LightSwitchable_Model : NetworkBehaviour
{
	public Light light;

	[SerializeField]
	private int flashProbability = 60;

	public bool state;
	
	// Model/Server
	// Gameplay
	void FixedUpdate()
	{
		if (IsServer)
		{
			if (Random.Range(1, flashProbability) == 1) // Server/Model
			{
				state = !state;
				
				LightChangePacket lightChangePacket;
				lightChangePacket           = new LightChangePacket();
				lightChangePacket.newState  = state;
				lightChangePacket.colour    = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
				lightChangePacket.intensity = Random.Range(1f, 100f);

				ChangeLightState_Rpc(lightChangePacket);
			}
		}
	}

	// View/Client
	// Just display of model data
	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable)]
	// private void ChangeLightState_Rpc(bool newState, Color colour, float intensity)
	private void ChangeLightState_Rpc(LightChangePacket packet)
	{
		if (IsClient)
		{
			// light.enabled = newState; // Client/View
			light.enabled = packet.newState;
			light.intensity = packet.intensity;
			light.color = packet.colour;
		}
	}
}