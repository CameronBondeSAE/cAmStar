using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetVar : NetworkBehaviour
{
	// View: CLIENT UI updating
	private NetworkVariable<float> anotherNumber = new NetworkVariable<float>(10f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

	public TextMeshProUGUI anotherNumberUI;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		if (IsClient)
			anotherNumber.OnValueChanged += OnValueChanged;

		Debug.Log("Just spawned: The value is " + anotherNumberUI.text);
		// Called manually on spawn, because the Netvar event DOESN'T FIRE on spawn.
		OnValueChanged(0, anotherNumber.Value);
	}

	public override void OnNetworkDespawn()
	{
		base.OnNetworkDespawn();

		if (IsClient)
			anotherNumber.OnValueChanged -= OnValueChanged;
	}

	private void OnValueChanged(float previousValue, float newValue)
	{
		anotherNumberUI.text = newValue.ToString();
	}


	public float           aNumber;
	public TextMeshProUGUI NumberUI;

	// Update is called once per frame
	void Update()
	{
		if (IsServer)
		{
			if (Input.GetKeyDown(KeyCode.N))
			{
				aNumber += Random.Range(-1f, 1f);

				anotherNumber.Value += Random.Range(-1f, 1f);

				ShowNumberUI_Rpc(aNumber);
			}
		}
	}

	[Rpc(SendTo.ClientsAndHost, Delivery = RpcDelivery.Unreliable)]
	public void ShowNumberUI_Rpc(float value)
	{
		NumberUI.text = value.ToString();
	}
}