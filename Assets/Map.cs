using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	private GridSpace[,] grid;
	public GameObject gridPrefab;
	public GameObject gridPrefabBlocked;
	public GameObject startPrefab;
	public GameObject targetPrefab;
	public Vector2Int start;
	public Vector2Int target;
	public Vector2Int size;

	// Start is called before the first frame update
	void Start()
	{
		SpawnGrid();
		RandomlyPositionStartAndTarget();
	}

	private void RandomlyPositionStartAndTarget()
	{
		Vector2 pos = FindUnblockedSpace();
		Instantiate(startPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
		pos = FindUnblockedSpace();
		Instantiate(targetPrefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
	}

	private Vector2 FindUnblockedSpace()
	{
		Vector2 randomPosition = new Vector2(0, 0);
		bool foundUnblockedSpace = false;
		while (foundUnblockedSpace == false)
		{
			int x = Random.Range(0, size.x - 1);
			int y = Random.Range(0, size.y - 1);
			randomPosition = new Vector2(x, y);
			if (grid[x, y].isBlocked == false)
			{
				foundUnblockedSpace = true;
				return new Vector2(x,y);
			}
		}

		return Vector2.negativeInfinity;
	}

	private void SpawnGrid()
	{
		// Spawn grid
		grid = new GridSpace[size.x, size.y];

		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				grid[x, y] = new GridSpace();
//				grid[x, y].isBlocked = Random.Range(0, 10) > 6; // HACK random map
				grid[x, y].isBlocked = Mathf.PerlinNoise(x/5f,y/5f)>0.5f; // HACK random map
				if (grid[x, y].isBlocked)
				{
					Instantiate(gridPrefabBlocked, new Vector3(x, 0, y), Quaternion.identity);
				}
				else
					Instantiate(gridPrefab, new Vector3(x, 0, y), Quaternion.identity);
			}
		}
	}
}
