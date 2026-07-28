using Anthill.AI;
using UnityEngine;

public class Chill_State : AntAIState
{
	public GameObject mainGameObject;

	public override void Create(GameObject aGameObject)
	{
		base.Create(aGameObject);
		
		mainGameObject = aGameObject;
	}

	public override void Enter()
	{
		base.Enter();
		
		Debug.Log(mainGameObject.name + " chilln");
	}
}
