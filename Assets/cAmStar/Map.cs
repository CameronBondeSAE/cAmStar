using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
	public Node[,] grid;
	public GameObject gridPrefab;
	public GameObject gridPrefabBlocked;
	public Vector2Int size;
	public Transform parentForDebugGOs;


	public bool occlusion = true;
	public bool hidingSpots = true;
	public float maxDistance = 25f;
	public float distanceToBeFullyOpen = 10f;
	public int numberOfAngles = 16;
	public AnimationCurve distanceToOcclusionScalar;

	int angleStep = 45;
	RaycastHit hitInfo;


	// Start is called before the first frame update
	void Awake()
	{
		GenerateNewMap();
		
		
		
//		// Scan the real world starting at 0,0,0 (to be able to place the grid add transform.position)
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				Collider[] overlapBox = Physics.OverlapBox(transform.position + new Vector3(x, 0, y), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, Int32.MaxValue, QueryTriggerInteraction.Collide);
				// if () // TODO: CHeck for cost zone
				// {
				// 	
				// 	// Something is there
				// 	grid[x, y].isBlocked = true;
				// 	grid[x, y].costMultiplier = // Put in cost zone value here
				// 	Gizmos.color         = Color.red;
				// 	Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), Vector3.one);
				// }
			}
		}
	}

	public void GenerateNewMap()
	{
		
		// if (grid != null && grid.Length > 0)
		// {
		// 	for (int x = 0; x < size.x; x++)
		// 	{
		// 		for (int y = 0; y < size.y; y++)
		// 		{
		// 			grid[x, y].isBlocked = false;
		// 		}
		// 	}
		// }
		SpawnGrid();
		Invoke("CalculateOcclusion_Coroutine", 2f);
	}

	private void CalculateOcclusion_Coroutine()
	{
		CalculateOcclusion();
	}


	public Vector2Int FindUnblockedSpace()
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
				return new Vector2Int(x, y);
			}
		}

		return new Vector2Int(-1, -1);
	}

	public void SpawnGrid()
	{
		// Destroy old maps debug GOs
		if (grid != null)
		{
			// for (int x = 0; x < size.x; x++)
			// {
			// 	for (int y = 0; y < size.y; y++)
			// 	{
			// 		Node node = grid[x, y];
			// 		if (node != null) Destroy(node.debugGO);
			// 	}
			// }

			if (parentForDebugGOs.childCount > 0)
			{
				foreach (Transform child in parentForDebugGOs.GetComponentsInChildren<Transform>())
				{
					if(child != parentForDebugGOs)
						Destroy(child.gameObject);
				}
			}
		}



		// for (int x = 0; x < size.x; x++)
		// {
		// 	for (int y = 0; y < size.y; y++)
		// 	{
		// 		grid[x, y].isBlocked = false;
		// 		grid[x, y].isCoverPoint = false;
		// 		grid[x, y].occlusionScore = 0f;
		// 		// grid[x, y] = 0f;
		// 	}
		// }

		// Spawn grid
		grid = new Node[size.x, size.y];

		float rndStart = Random.Range(0, 1000f);

		GameObject o;


		// Border
		for (int x = -1; x < size.x + 1; x++)
		{
			o = Instantiate(gridPrefabBlocked, new Vector3(x, 0, -1), Quaternion.identity, parentForDebugGOs);
			o.GetComponentInChildren<Renderer>().material.color = Color.blue;
		}

		for (int x = -1; x < size.x + 1; x++)
		{
			o = Instantiate(gridPrefabBlocked, new Vector3(x, 0, size.y), Quaternion.identity, parentForDebugGOs);
			o.GetComponentInChildren<Renderer>().material.color = Color.blue;
		}

		for (int y = -1; y < size.y + 1; y++)
		{
			o = Instantiate(gridPrefabBlocked, new Vector3(-1, 0, y), Quaternion.identity, parentForDebugGOs);
			o.GetComponentInChildren<Renderer>().material.color = Color.blue;
		}

		for (int y = -1; y < size.y + 1; y++)
		{
			o = Instantiate(gridPrefabBlocked, new Vector3(size.x, 0, y), Quaternion.identity, parentForDebugGOs);
			o.GetComponentInChildren<Renderer>().material.color = Color.blue;
		}


		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				grid[x, y] = new Node();
				grid[x, y].position = new Vector2Int(x, y);
//grid[x, y].isBlocked = Random.Range(0, 10) > 6; // HACK random map
				grid[x, y].isBlocked = Mathf.PerlinNoise(rndStart + x / 10f, y / 10f) > 0.5f; // HACK random map
				if (grid[x, y].isBlocked)
				{
					o = Instantiate(gridPrefabBlocked, new Vector3(x, 0, y), Quaternion.identity, parentForDebugGOs);
					o.GetComponentInChildren<Renderer>().material.color = Color.red;
				}
				else
				{
					o = Instantiate(gridPrefab, new Vector3(x, 0, y), Quaternion.identity, parentForDebugGOs);
					o.GetComponentInChildren<Renderer>().material.color = Color.green;
					// HACK debug
				}

				grid[x, y].debugGO = o;
			}
		}
	}


	public void CalculateOcclusion()
	{
		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				grid[x, y].occlusionScore = 0;
			}
		}
// return;
		angleStep = 360 / numberOfAngles;

		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				// 2d to 3d space. Y to Z
				Ray ray = new Ray(new Vector3(grid[x, y].position.x, 0, grid[x, y].position.y), Vector3.zero);

				grid[x, y].occlusionScore = 0;

				for (int angle = 0; angle < 360; angle = angle + angleStep)
				{
					// Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.up);

					Vector3 dir = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
					ray.direction = dir;

					bool hit = Physics.Raycast(ray, out hitInfo, maxDistance);
					if (hit)
					{
						// Bail if there's a long distance to be seen anywhere
						if (hitInfo.distance > distanceToBeFullyOpen && hidingSpots == true)
						{
							grid[x, y].occlusionScore = 0;
							break;
						}

						if (occlusion)
						{
							grid[x, y].occlusionScore +=
								distanceToOcclusionScalar.Evaluate((maxDistance - hitInfo.distance) / maxDistance);
						}
					}

					// grid[x, y].position
				}

				// Debug.Log(grid[x, y].occlusionScore);
			}
		}
	}

	public void OnDrawGizmos()
	{
		float maxOcclusionValue = numberOfAngles;

		for (int x = 0; x < size.x; x++)
		{
			for (int y = 0; y < size.y; y++)
			{
				if (grid != null)
				{
					if (grid[x, y].isBlocked)
					{
						// Gizmos.color = Color.red;
						// Gizmos.DrawCube(new Vector3(x, 0, y), Vector3.one);
					}
					else
					{
						// numberOfAngles
						// numberOfAngles* maxDistance;

						// CHECK: Slow to change colours so often
						// Scale the value to a 0 to 255 value for colours.
						float occlusionScore = 1f - (grid[x, y].occlusionScore / maxOcclusionValue);
						Gizmos.color = new Color(occlusionScore, occlusionScore, occlusionScore);
						// Gizmos.color = new Color(1f,1f,1f);
						Gizmos.DrawCube(new Vector3(x, 0, y), new Vector3(1, 0.1f, 1));
					}
				}
			}
		}


	}
}