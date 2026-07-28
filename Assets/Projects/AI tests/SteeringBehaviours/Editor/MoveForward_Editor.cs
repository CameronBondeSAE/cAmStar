using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveForward), true)]
public class MoveForward_Editor : Editor
{
	public void OnSceneGUI()
	{
		MoveForward moveForward;
		moveForward = target as MoveForward;

		if (moveForward != null)
		{
			Handles.Label(moveForward.transform.position + new Vector3(0,1.5f,0), moveForward.camsMessage);
		}

		Handles.color = Color.red;
		Handles.DrawLine(moveForward.transform.position, moveForward.transform.position + moveForward.transform.up * 10f);
	}
}