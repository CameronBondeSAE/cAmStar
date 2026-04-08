using System.Collections;
using Anthill.AI;
using UnityEngine;

public class RunAwayFromChaos_State : AntAIState
{
	public GameObject mainGameObject;

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);

		// Just so other functions can access the main go without hacky getcomponent stuff
		mainGameObject = aGameObject;
	}

	public override void Enter()
	{
		base.Enter();

		Debug.Log(mainGameObject.name + "RunAwayFromChaos_State");

		// Invoke("WaitForABit", 5f);
		GetComponentInParent<CamGuy_Sensor>().seesChaos = false;
	}

	public override void Execute(float aDeltaTime, float aTimeScale)
	{
		base.Execute(aDeltaTime, aTimeScale);

		float     panicStrength = mainGameObject.GetComponent<CamGuy_Model>().panicStrength;
		Rigidbody rb            = mainGameObject.GetComponent<Rigidbody>();

		rb.AddForce(new Vector3(-panicStrength + Random.value * panicStrength, 0f,
		                        -panicStrength + Random.value * panicStrength));
		rb.AddTorque(new Vector3(0f, -panicStrength + Random.value * panicStrength, 0f));
	}

	private void WaitForABit()
	{
	}
}