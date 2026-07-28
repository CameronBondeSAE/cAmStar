using System;
using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
	// COMMENT
	[SerializeField]
	private float radius = 10f;

	Collider[] collidersInRange;

	private void Awake()
	{
		collidersInRange = new Collider[10];
	}

	public void EmitSound()
	{
		if (Physics.OverlapSphereNonAlloc(transform.position, radius, collidersInRange) > 0)
		{
			foreach (Collider col in collidersInRange)
			{
				if (col == null)
				{
					continue;
				}

				Ear ear = col.GetComponentInChildren<Ear>();

				if (ear != null)
				{
					ear.HeardSomething(this);
				}
			}
		}
	}

}
