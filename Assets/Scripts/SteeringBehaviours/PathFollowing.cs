using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Netcode.Samples.SteeringBehaviours
{
	public class PathFollowing : MonoBehaviour
	{
		public Color pathColor;

		private void Awake()
		{
			pathColor = new Color(Random.value, Random.value, Random.value);
		}

		private void OnDrawGizmosSelected()
		{
			for (int i = 0; i < 50; i++)
			{
				Gizmos.color = pathColor;
				Gizmos.DrawLine(transform.position,
				                transform.position +
				                new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
			}
		}
	}
}