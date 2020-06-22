using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
	public Node[,]    grid;
	public GameObject gridPrefab;
	public GameObject gridPrefabBlocked;
	public Vector2Int size;

	// Start is called before the first frame update
	void Awake()
	{
		SpawnGrid();
	}


	public Vector2Int FindUnblockedSpace()
	{
		Vector2 randomPosition      = new Vector2(0, 0);
		bool    foundUnblockedSpace = false;
		while (foundUnblockedSpace == false)
		{
			int x = Random.Range(0, size.x - 1);
			int y = Random.Range(0, size.y - 1);
			randomPosition = new Vector2(x, y);
			if (grid[x, y].isBlocked == false)
			{
				foundUnblockedSpace = true;
				return new Vector2Int(x, y);
			}
		}

		return new Vector2Int(-1, -1);
	}

	private void SpawnGrid()
	{
		// Spawn grid

		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				if (Random.value > 0.5f)
				{
					Debug.DrawLine(new Vector3(x,0,y), new Vector3(x,5,y), Color.red, 100f);
				}
				else
				{
					Debug.DrawLine(new Vector3(x,0,y), new Vector3(x,2,y), Color.green, 100f);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				if (grid != null)
				{
					if (grid[x, y].isBlocked)
					{
						Gizmos.color = Color.red;
						Gizmos.DrawCube(new Vector3(x, 0, y), Vector3.one);
					}
				}
			}
		}


//		// Scan the real world starting at 0,0,0 (to be able to place the grid add transform.position)
//		for (int x = 0; x < size.x; x++)
//		{
//			for (int y = 0; y < size.y; y++)
//			{
//				if (Physics.CheckBox(transform.position + new Vector3(x, 0, y), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity))
//				{
//					// Something is there
//					grid[x, y].isBlocked = true;
//					Gizmos.color = Color.red;
//					Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), Vector3.one);
//				}
//			}
//		}
	}
}