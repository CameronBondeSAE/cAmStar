using Anthill.AI;
using UnityEngine;

public enum CamGuyBrain
{
	seesChaos  = 0,
	hearsChaos = 1
}

public class CamGuy_Sensor : MonoBehaviour, ISense
{
	// Convenience bools for inspector debugging
	public bool seesChaos;
	public bool hearsChaos;

	// Actual code to detect things
	public Vision vision;
	public Ear    ear;
	
    public void CollectConditions(AntAIAgent aAgent, AntAICondition aWorldState)
    {
	    // Just passing along the bools, so we can see click the inspector to force it, for testing
	    seesChaos = vision.seesSomething;
	    hearsChaos = ear.heardSomething;
	    
	    aWorldState.Set(CamGuyBrain.seesChaos, seesChaos);
	    aWorldState.Set(CamGuyBrain.hearsChaos, hearsChaos);
    }
}
