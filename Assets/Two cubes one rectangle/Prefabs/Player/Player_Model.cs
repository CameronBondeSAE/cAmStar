using System;
using UnityEngine;

/// <summary>
/// Non-networked model code (only view is networked atm)
/// </summary>
public class Player_Model : MonoBehaviour
{
	public  Rigidbody2D rb;
	[SerializeField]
	private float horizontalPushScale = 1f;

	/// <summary>
	/// Jump height is scaled by the hold time
	/// </summary>
	[SerializeField]
	private float jumpHeightScale = 1f;

	[SerializeField]
	private float backwardsRollSpeed = 0.05f;

	public event Action StartJump_Event;
	public event Action Jump_Event;

	public bool isStartingJump = false;

	public float currentScale;
	public float holdTime;

	private void FixedUpdate()
	{
		if (isStartingJump)
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
		isStartingJump = true;
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

	    // Stop shrinking
	    isStartingJump = false;
	    currentScale = 1f;
	    transform.localScale = Vector3.one;
	    
	    // Jump force
	    rb.AddForce(new Vector2(holdTime * horizontalPushScale, holdTime * jumpHeightScale), ForceMode2D.Impulse);

	    // Anyone who cares, can subscribe to this event eg the view scripts
	    if (Jump_Event != null)
	    {
		    Jump_Event();
	    }
    }
}
