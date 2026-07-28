using Unity.Netcode;
using UnityEngine;

namespace TwoCubes
{
	public class Spawner_Model : NetworkBehaviour
	{
		public GameObject prefab;

		[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable)]
		public void Spawn_Rpc()
		{
			GameObject newGO = Instantiate(prefab, transform.position, transform.rotation);
			newGO.GetComponent<NetworkObject>().Spawn();
			
			MakeThisThingHuge(newGO.GetComponent<NetworkObject>());
		}

		private void MakeThisThingHuge(NetworkObjectReference netObjectReference)
		{
			// Check this exists on the clients
			if (netObjectReference.TryGet(out NetworkObject netObject))
			{
				netObject.gameObject.transform.localScale = Vector3.one * 10f;
			}
		}
	}
}