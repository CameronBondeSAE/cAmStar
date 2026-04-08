using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class CamGuy_Model : MonoBehaviour
{
	public float panicStrength = 1000f;

	public List<Memory> memories;


	private void Update()
	{
		Memory memory = new Memory();
		// memory.door     = thing;
		// memory.position = thingISaw.transform.position;
		
		// soundEmitter.Emit(memory, position)
		
		memories.Add(memory);
	}
}

public class Memory
{
	public Vector3        position;
	public Door           door;
	// public EnergyResource energyResource;
	public Player         player;
}
