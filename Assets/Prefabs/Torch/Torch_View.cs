using System;
using Unity.Netcode;
using UnityEngine;

public class Torch_View : NetworkBehaviour
{
	public Light       light;
	public Torch_Model torchModel;

	// Subscribe to model events, so the model stays clean and logic ONLY.
	// View stuff should REACT to model events
	private void OnEnable()
	{
		// React to the model data changing
		if (IsClient)
		{
			torchModel.currentOnState.OnValueChanged += ActivateLight;
		}
	}

	private void OnDisable()
	{
		// UnSubscribe to the model data
		if (IsClient)
		{
			torchModel.currentOnState.OnValueChanged -= ActivateLight;
		}
	}

	// Server to clients. THEN the server syncs the VIEW to ALL CLIENTS
	public void ActivateLight(bool state, bool newValue)
	{
		light.enabled = state;
		Debug.Log("View = "+state);
	}
}