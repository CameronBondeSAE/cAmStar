using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Non-networked model code (only view is networked atm)
/// </summary>
public class Player_Model : NetworkBehaviour
{
	public  Rigidbody2D rb;
	[SerializeField]
	private float horizontalPushScale = 1f;
	
	public NetworkPositionRotationPhysics networkPositionRotationPhysics;

	/// <summary>
	/// Jump height is scaled by the hold time
	/// </summary>
	[SerializeField]
	private float jumpHeightScale = 1f;

	[SerializeField]
	private float backwardsRollSpeed = 0.05f;

	public event Action StartJump_Event;
	public event Action Jump_Event;

	public NetworkVariable<bool> isStartingJump = new NetworkVariable<bool>(false);

	public float currentScale;
	public float holdTime;

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		isStartingJump.OnValueChanged += OnValueChanged;
	}

	override public void OnNetworkDespawn()
	{
		base.OnNetworkDespawn();
		isStartingJump.OnValueChanged -= OnValueChanged;
	}

	private void OnValueChanged(bool previousValue, bool newValue)
	{
		if (newValue == true && previousValue == false)
		{
			// Stop shrinking
			isStartingJump.Value = false;
			currentScale         = 1f;
			transform.localScale = Vector3.one;
		}
	}

	private void FixedUpdate()
	{
		if (isStartingJump.Value)
		{
			// Shrink over time
			currentScale -= Time.fixedDeltaTime * 1f;
			
			currentScale         = Mathf.Clamp(currentScale, 0.25f, 1f);
			transform.localScale = new Vector3(currentScale, currentScale, currentScale);
			
			// Spin
			rb.AddTorque(-holdTime * backwardsRollSpeed);
		}
	}

	public void StartJump()
	{
		isStartingJump.Value = true;
		holdTime       = Time.timeSinceLevelLoad;

		// Anyone who cares, can subscribe to this event eg the view scripts
		if (StartJump_Event != null)
		{
			StartJump_Event();
		}
	}
	
	public void Jump()
    {
	    // How big is my jump going to be? Longer = Higher
	    holdTime = Time.timeSinceLevelLoad - holdTime;

	    // Jump force
	    rb.AddForce(new Vector2(holdTime * horizontalPushScale, holdTime * jumpHeightScale), ForceMode2D.Impulse);

	    // Update the network NOW (if tickrate allows it, ie it puts it in the queue)
	    networkPositionRotationPhysics.ForceUpdatePositionRotationPhysics();
	    
	    // Anyone who cares, can subscribe to this event eg the view scripts
	    if (Jump_Event != null)
	    {
		    Jump_Event();
	    }
    }
}
