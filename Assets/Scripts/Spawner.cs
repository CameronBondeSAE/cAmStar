using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;
	
	[SerializeField]
	private int amount = 10;
	
	[SerializeField]
	private float range = 5f;

	[SerializeField]
	private bool spawnOnStart = true;
	
	private void Start()
	{
		if (spawnOnStart)
		{
			Spawn();
		}
	}

	public void Spawn()
    {
	    for (int i = 0; i < amount; i++)
	    {
		    Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
		    Instantiate(prefab, transform.position + new Vector3(Random.Range(-range, range), 0, Random.Range(-range,range)), rotation);
	    }
    }
}
