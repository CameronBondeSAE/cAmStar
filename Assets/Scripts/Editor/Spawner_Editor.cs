using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner), true)]
public class Spawner_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying)
		{
			if (GUILayout.Button("Spawn"))
			{
				// ‘target’ is the magic variable that editors use to link back to the original component. It’s in the BASE CLASS, so you have to ‘cast’ to get access to YOUR functions.
				Spawner Spawner;
				Spawner = target as Spawner;
				Spawner?.Spawn();
			}
		}
	}
}
