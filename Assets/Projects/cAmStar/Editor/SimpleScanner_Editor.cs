using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleScanner), true)]
public class SimpleScanner_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (Application.isPlaying)
		{
			if (GUILayout.Button("Scan"))
			{
				SimpleScanner simpleScanner = target as SimpleScanner;
				if (simpleScanner != null)
				{
					simpleScanner.Scan();
				}
			}
		}
	}
}