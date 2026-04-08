using System;
using System.Collections.Generic;
using UnityEngine;

public class Neighbours : MonoBehaviour
{
	public List<Transform> neighboursList;

	private void OnTriggerEnter(Collider other)
	{
		neighboursList.Add(other.transform);
	}

	private void OnTriggerExit(Collider other)
	{
		neighboursList.Remove(other.transform);
	}
}