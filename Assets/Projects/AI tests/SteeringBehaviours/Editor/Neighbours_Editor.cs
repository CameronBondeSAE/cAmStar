using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

[CustomEditor(typeof(Neighbours), true)]
public class Neighbours_Editor : Editor
{
	private void OnSceneGUI()
	{
		Neighbours neighbours = target as Neighbours;

		Handles.color = Color.cyan;
		Transform neighboursTransform = neighbours.transform;
		// Handles.DrawSolidArc(neighboursTransform.position, neighboursTransform.up, neighboursTransform.forward, 220, 10);
		// Handles.DrawLine(neighboursTransform.position, neighboursTransform.position + neighboursTransform.up * 220);
		Handles.ArrowHandleCap(0, neighboursTransform.position, Quaternion.LookRotation(neighboursTransform.forward), 3, EventType.Repaint);
	}
}
