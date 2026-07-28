using System;
using System.Collections;
using UnityEngine;

public class Ear : MonoBehaviour
{
	public bool  heardSomething;
	public float timeToReset = 5f;

	public void HeardSomething(SoundEmitter thingThatEmittedSound)
	{
		// Set the ‘lastHeard’ variable
		// 	Emit event, so other model code can react. Basically just forward on the SoundEmitter variable through an event.
		// Eg Your sensor planner code might want to set a condition ‘heardSound’

		Debug.Log("Heard something! : " + thingThatEmittedSound.name);
		Debug.DrawLine(thingThatEmittedSound.transform.position, transform.position, Color.red, 4f);

		heardSomething = true;

		// This should eventually set back to false
		Invoke("Reset", timeToReset);
	}

	private void Reset()
	{
		heardSomething = false;
	}
}