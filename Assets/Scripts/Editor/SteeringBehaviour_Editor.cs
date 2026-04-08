using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SteeringBehaviour_Manager), true)]
public class SteeringBehaviour_Manager_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Debug all Steering Behaviours"))
		{
			// ‘target’ is the magic variable that editors use to link back to the original component. It’s in the BASE CLASS, so you have to ‘cast’ to get access to YOUR functions.
			SteeringBehaviour_Manager SteeringBehaviour_Manager;
			SteeringBehaviour_Manager = target as SteeringBehaviour_Manager;
			SteeringBehaviour_Manager?.DebugAllBehaviours();
		}
	}
}