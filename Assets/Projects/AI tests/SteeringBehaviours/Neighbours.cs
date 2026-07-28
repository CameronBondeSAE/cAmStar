using System;
using System.Collections.Generic;
using UnityEngine;

public class Neighbours : MonoBehaviour
{
	public List<Transform> neighboursList;

	private void OnTriggerEnter(Collider other)
	{
		if(other.GetComponent<CamsGuy_Model>())
			neighboursList.Add(other.transform);
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.GetComponent<CamsGuy_Model>())
			neighboursList.Remove(other.transform);
	}
}