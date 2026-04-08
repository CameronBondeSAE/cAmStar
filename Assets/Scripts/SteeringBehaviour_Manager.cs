using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour_Manager : MonoBehaviour
{
	[Tooltip("DOESN'T DO ANYTHING")]
	[Header("Spawner Settings")]
	public int things;

	[Space(40)]
	[Range(3, 100)]
	public float other;
	
	public List<SteeringBehaviour_Base> steeringBehaviours;
	public List<SteeringBehaviour_Base> attackBehaviours;

	public void DebugAllBehaviours()
	{
		// ALL steering behaviours in the world!
		foreach (SteeringBehaviour_Base item in FindObjectsByType<SteeringBehaviour_Base>(FindObjectsInactive.Include, FindObjectsSortMode.None))
		{
			// Casting
			Avoid avoid = item as Avoid;
			if (avoid != null)
			{
				avoid.turnSpeed = 200;
			}

			Debug.Log("GO = " + item.gameObject.name + " : Behaviour = " + item.ToString());
		}
	}
	
	

	public void ToggleMovement()
	{
		foreach (MoveForward item in FindObjectsByType<MoveForward>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
		{
			item.enabled = !item.enabled;
		}
	}
}
