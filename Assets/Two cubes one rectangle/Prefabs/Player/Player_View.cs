using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Networked view effects
/// </summary>
public class Player_View : NetworkBehaviour
{
	public Player_Model  playerModel;
	public MeshRenderer  meshRenderer;
	public Color         jumpColour = Color.green;
	public Color         startingJumpColour = Color.red;
	public AudioResource jumpSound;
	public AudioResource startJumpSound;

	public AudioSource audioSource;

	private void OnEnable()
	{
		// Subscribe to Jump. WHEN this happens, run my function (not now)
		playerModel.Jump_Event      += PlayerModelOnJump_Event_Rpc;
		playerModel.StartJump_Event += PlayerModelOnStartJump_Event_Rpc;
	}

	private void OnDisable()
	{
		// UN-Subscribe to Jump. So when we're destroyed, it doesn't try and run this code.
		playerModel.Jump_Event -= PlayerModelOnJump_Event_Rpc;
		playerModel.StartJump_Event -= PlayerModelOnStartJump_Event_Rpc;
	}

	/// <summary>
	/// Run on all clients for preparing to jump effects
	/// </summary>
	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Unreliable)]
	private void PlayerModelOnStartJump_Event_Rpc()
	{
		Debug.Log("PlayerModelOnStartJump_Event");
		audioSource.resource = startJumpSound;
		audioSource.Play();
		
		meshRenderer.material.color = startingJumpColour;
	}

	/// <summary>
	/// Runs on all clients for jump effects
	/// </summary>
	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Unreliable)]
	private void PlayerModelOnJump_Event_Rpc()
	{
		Debug.Log("PlayerModelOnJumpEvent");
		audioSource.resource = jumpSound;
		audioSource.Play();

		meshRenderer.material.color = jumpColour;

		// Wait a bit to change it back
		Invoke("ResetColour", 0.2f);
	}

	private void ResetColour()
	{
		meshRenderer.material.color = Color.white;
	}
}