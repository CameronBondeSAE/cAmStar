using UnityEditor;
using UnityEngine;

namespace TwoCubes
{
	[CustomEditor(typeof(Spawner_Model), true)]
	public class Spawner_Editor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.Space(40);
			GUILayout.Label("*** Debugging*** ");
			if (GUILayout.Button("Spawn"))
			{
				Debug.Log("Spawning");
				
				// Cast to actual component. Fails if it's not the proper one
				Spawner_Model spawnerModel = target as Spawner_Model;
				spawnerModel?.Spawn_Rpc();
			}
		}
	}
}