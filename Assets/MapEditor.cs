using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		// DrawDefaultInspector();
		bool button = GUILayout.Button("Recalculate occlusion");
		if (button)
		{
			((Map)target).CalculateOcclusion();
		}
	}
}
