using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
	public NetworkObject   playerPrefab;
	public List<Transform> spawnPoints = new List<Transform>();
	public int             spawnIndex;
	public TrackingCamera          trackingCamera;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		if (!NetworkManager.IsServer)
		{
			return;
		}

		Debug.Log("Spawned GameManager");

		NetworkManager.OnClientConnectedCallback += NetworkManagerOnOnClientConnectedCallback;
	}

	public override void OnNetworkDespawn()
	{
		base.OnNetworkDespawn();

		NetworkManager.OnClientConnectedCallback += NetworkManagerOnOnClientConnectedCallback;
	}


	private void NetworkManagerOnOnClientConnectedCallback(ulong obj)
	{
		SpawnPlayer(obj);
	}

	public void SpawnPlayer(ulong clientId)
	{
		NetworkObject newPlayer = Instantiate(playerPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
		newPlayer.SpawnAsPlayerObject(clientId);
		
		spawnIndex++;
		if (spawnIndex >= spawnPoints.Count)
		{
			spawnIndex = 0;
		}

		if (newPlayer != null)
		{
			AssignCamera_Rpc(newPlayer);
		}
	}

	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Reliable)]
	private void AssignCamera_Rpc(NetworkObjectReference newPlayer)
	{
		newPlayer.TryGet(out NetworkObject networkObject);
		if(networkObject.IsLocalPlayer)
			trackingCamera.targetTransform = networkObject.transform;
	}


	private void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
	{
		Debug.Log("Connection event	- ClientID = " + arg2.ClientId);

		switch (arg2.EventType)
		{
			case ConnectionEvent.ClientConnected:
				Debug.Log("Client connected");
				// NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(playerPrefab, arg2.ClientId, false, true);
				break;
			case ConnectionEvent.PeerConnected:
				Debug.Log("Peer Connected");
				break;
			case ConnectionEvent.ClientDisconnected:
				Debug.Log("Peer ClientDisconnected");
				break;
			case ConnectionEvent.PeerDisconnected:
				Debug.Log("Peer Disconnected");
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}