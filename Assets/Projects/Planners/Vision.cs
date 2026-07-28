using System;
using UnityEngine;

public class Vision : MonoBehaviour
{
	public bool seesSomething = false;
	
	private void OnTriggerEnter(Collider other)
	{
		seesSomething = true;
	}

	private void OnTriggerExit(Collider other)
	{
		seesSomething = false;
	}
}
