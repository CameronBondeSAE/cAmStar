using System;
using UnityEngine;

[Serializable]
public class Node
{
	public bool isBlocked = false;
	public Node parent;
	public int gCost;
	public int hCost;
	public int fCost;

	public Vector2Int position;

	// HACK debug
	public GameObject debugGO;
}
