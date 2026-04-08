using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Unity.Netcode.Samples;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviour
{
	public string             lobbyName                    = "Cam's game of Two Squares, One Rectangle";
	public int                maxPlayers                   = 4;
	public CreateLobbyOptions options                      = new CreateLobbyOptions();
	public string             relayCode                    = "FAKE";
	public string             hackHardcodedLobbyNameToJoin = "Cam's game of Two Squares, One Rectangle";

	public Transform  lobbyCanvasParent;
	public GameObject lobbyPrefab;

	// private void Start()
	// {
	// 	for (int i = 0; i < 10; i++)
	// 	{
	// 		GameObject indivualLobbyUI = Instantiate(lobbyPrefab, lobbyCanvasParent);
	// 	}
	// }
	
	public BootstrapManager bootstrapManager;


	public async void CreateLobby()
	{
		await bootstrapManager.StartHostWithRelay(4, "udp");
		
		options.IsPrivate = false;

		// Very spaced out creation of custom data for the lobby. In this the relay code (but could be lobby name, player count, map name etc)
		options.Data = new Dictionary<string, DataObject>();
		DataObject dataObject;
		dataObject = new DataObject(DataObject.VisibilityOptions.Public, bootstrapManager.joinCode, DataObject.IndexOptions.S1);
		options.Data.Add("RelayCode", dataObject);

		Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

		if (lobby != null)
		{
			Debug.Log("CREATED LOBBY : " + lobby.Name);
		}
		
		// Update the UI (bit HACK should really locally update it so we don't hassle the Unity Lobby server)
		Query();

		// Heartbeat the lobby every 15 seconds.
		StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));
	}

	
	public async void JoinLobby(string lobbyId, string relayJoinCode)
	{
		Lobby joinedLobby = null;
		try
		{
			// HACK: TODO: Query the real server!!!
			joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
		}

		if (joinedLobby != null)
		{
			Debug.Log("JOINED LOBBY : " + joinedLobby.Name);
			// Debug.Log(joinedLobby.Data);

			foreach (KeyValuePair<string, DataObject> keyValuePair in joinedLobby.Data)
			{
				Debug.Log("Key = "+keyValuePair.Key + " : Value = "+keyValuePair.Value.Value);
			}
		}

		await bootstrapManager.StartClientWithRelay(relayJoinCode, "udp");
		
		// Update the UI (bit HACK should really locally update it so we don't hassle the Unity Lobby server)
		Query();
	}

	public async void QuickJoin()
	{
		try
		{
			// Quick-join a random lobby with a maximum capacity of 10 or more players.
			QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

			options.Filter = new List<QueryFilter>()
			                 {
				                 new QueryFilter(
				                                 field: QueryFilter.FieldOptions.MaxPlayers,
				                                 op: QueryFilter.OpOptions.GE,
				                                 value: "10")
			                 };

			Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);

			// ...
		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
		}
	}


	public async void Query()
	{
		try
		{
			QueryLobbiesOptions options = new QueryLobbiesOptions();
			options.Count = 25;

			// Filter for open lobbies only
			// options.Filters = new List<QueryFilter>();
			// options.Filters.Add(new QueryFilter(
			//                                     field: QueryFilter.FieldOptions.AvailableSlots,
			//                                     op: QueryFilter.OpOptions.GT,
			//                                     value: "0"));

			// Order by newest lobbies first
			options.Order = new List<QueryOrder>()
			                {
				                new QueryOrder(
				                               asc: false,
				                               field: QueryOrder.FieldOptions.Created)
			                };

			QueryResponse lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);


			// Debugging
			Debug.Log("*************************************");
			Debug.Log("Lobbies found : " + lobbies.Results.Count);
			
			// Clear out the existing entries (You could check if they already exist and update them, I'm lazy)
			for (int i = 0; i < lobbyCanvasParent.transform.childCount; i++)
			{
				Destroy(lobbyCanvasParent.transform.GetChild(i).gameObject);
			}

			foreach (Lobby lobbiesResult in lobbies.Results)
			{
				Debug.Log("-------------------------------------------------------------");
				Debug.Log(lobbiesResult.Name);
				Debug.Log("Created = " + lobbiesResult.Created);
				Debug.Log("MaxPlayers = " + lobbiesResult.MaxPlayers);
				Debug.Log("AvailableSlots = " + lobbiesResult.AvailableSlots);
				Debug.Log("IsPrivate = " + lobbiesResult.IsPrivate);

				foreach (KeyValuePair<string, DataObject> keyValuePair in lobbiesResult.Data)
				{
					Debug.Log("		- Lobby data name = " + keyValuePair.Key + " : Value = " + keyValuePair.Value.Value);
				}

				foreach (Unity.Services.Lobbies.Models.Player player in lobbiesResult.Players)
				{
					Debug.Log("		- Player = " + player.Id);
				}
				
				
				//... Do UI stuff here
				// ALSO hook up the JOIN button with the lobby IDs to LATER ON you can join them when the player clicks it
			
				GameObject individualLobbyUI = Instantiate(lobbyPrefab, lobbyCanvasParent);
				individualLobbyUI.GetComponent<LobbyPanel_Model>().lobbyName.text = lobbiesResult.Name;
				individualLobbyUI.GetComponent<LobbyPanel_Model>().lobbyPlayers.text = lobbiesResult.Players.Count.ToString();
				individualLobbyUI.GetComponent<LobbyPanel_Model>().lobbyId = lobbiesResult.Id;
				
				// Join button needs to STORE the lobbyID for LATER if the player clicks it
				// This is called a "lambda expression"
				// This temporary storage of the parameters until it's needed LATER, is called a "Closure"
				// The "lobbiesResult.Data["RelayCode"].Value" part is MY custom data name "RelayCode". I'm reading that entry out of the lobby server and passing it to the Join Button for later joining of the relay server
				individualLobbyUI.GetComponent<LobbyPanel_Model>().joinButton.onClick.AddListener(() => JoinLobby(lobbiesResult.Id, lobbiesResult.Data["RelayCode"].Value));
			}

		}
		catch (LobbyServiceException e)
		{
			Debug.Log(e);
		}
	}


	ConcurrentQueue<string> createdLobbyIds = new ConcurrentQueue<string>();

	void OnApplicationQuit()
	{
		while (createdLobbyIds.TryDequeue(out var lobbyId))
		{
			LobbyService.Instance.DeleteLobbyAsync(lobbyId);
		}
	}

	IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
	{
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitTimeSeconds);

		while (true)
		{
			LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
			Debug.Log("Heartbeat PONG : " + lobbyId);
			yield return delay;
		}
	}
}