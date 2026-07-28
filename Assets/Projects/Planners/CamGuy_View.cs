using System;
using UnityEngine;

public class CamGuy_View : MonoBehaviour
{
	public CamGuy_Model camGuy_Model;

	private void OnEnable()
	{
		camGuy_Model.Damage_Event += CamGuy_ModelOnDamage_Event;
	}

	private void OnDisable()
	{
		camGuy_Model.Damage_Event -= CamGuy_ModelOnDamage_Event;
	}

	private void CamGuy_ModelOnDamage_Event()
	{
		Debug.Log("CamGuy_ModelOnDamage_Event");
	}
}
