using System;
using UnityEngine;

public class Vision : MonoBehaviour
{
	public CamGuy_Sensor camGuy_Sensor;
	
	private void OnTriggerEnter(Collider other)
	{
		camGuy_Sensor.seesChaos = true;
	}

	private void OnTriggerExit(Collider other)
	{
		camGuy_Sensor.seesChaos = false;
	}
}
