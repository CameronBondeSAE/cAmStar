using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundEmitter), true)]
public class SoundEmitter_Editor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		SoundEmitter soundEmitter;
		soundEmitter = target as SoundEmitter;

		if (GUILayout.Button("Emit"))
		{
			soundEmitter?.EmitSound();
		}
	}
}
