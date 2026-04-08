using UnityEngine;

public class CameraTracker : MonoBehaviour
{
	public Transform target;
	public Vector3 offset;
	
    void LateUpdate()
    {
	    transform.position = target.position + offset;
	    transform.LookAt(target);
    }
}
