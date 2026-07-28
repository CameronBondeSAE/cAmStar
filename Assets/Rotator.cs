using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float speed = 1f;
	
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, speed, 0));
    }
}
