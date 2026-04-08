using Anthill.AI;
using UnityEngine;

public enum CamGuyBrain
{
	seesChaos = 0
}

public class CamGuy_Sensor : MonoBehaviour, ISense
{
	public bool seesChaos;
	
    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
	    aWorldState.Set(CamGuyBrain.seesChaos, seesChaos);
    }
}
