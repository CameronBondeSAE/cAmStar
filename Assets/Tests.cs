using System;
using UnityEngine;
using Object = System.Object;

public class Tests : MonoBehaviour
{
	public GameObject    thingToDestroy;
	public Object anyScript;
	private event Action OnEventTest;
	public Func<int>     myFunc;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	    myFunc = MyFunction;
	    
        Destroy(thingToDestroy);
        
        // OnEventTest += MyFunction;

        myFunc();
    }

    private int MyFunction()
    {
	    Debug.Log("MyFunction");

	    return 0;
    }

    // Update is called once per frame
    void Update()
    {
	    OnEventTest?.Invoke();
	    
	    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo))
	    {
		    Debug.Log("Distance = " + hitInfo.distance);
	    
		    // Advanced for wall angles etc
		    Debug.Log("Normal = " + hitInfo.normal);
		    Debug.Log("Reflect angle = " + Vector3.Angle(hitInfo.normal, transform.forward));
		    Debug.Log("Dot = "+Vector3.Dot(transform.forward, hitInfo.normal));
	    
		    Debug.DrawLine(transform.position, hitInfo.point, Color.green);
		    Debug.DrawRay(hitInfo.point, Vector3.Reflect(transform.forward, hitInfo.normal) * 5f, Color.cyan);
		    Debug.DrawRay(hitInfo.point, Vector3.Cross(transform.forward, hitInfo.normal) * 5f, Color.magenta);
	    }
    }
}
