using System.Collections;
using UnityEngine;

public class SimpleScanner : MonoBehaviour
{
	public int     width;
	public int     length;
	public Node[,] gridNodeReferences;
	public bool    constantlyScan = false;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridNodeReferences = new Node[width,length];

        if (constantlyScan)
        {
	        StartCoroutine(ScanContinously());
        }
        else
        {
	        Scan();
        }
    }

    private IEnumerator ScanContinously()
    {
	    while (true)
	    {
		    Scan();
		    yield return new WaitForSeconds(0.5f);
	    }
    }

    public void Scan()
    {
	    for (int x = 0; x < width; x++)
	    {
		    for (int z = 0; z < length; z++)
		    {
			    gridNodeReferences[x, z] = new Node();
			    
			    if (Physics.CheckBox(new Vector3(x, 0, z),
			                         new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity))
			    {
				    gridNodeReferences[x, z].isBlocked = true;
			    }
		    }
	    }
    }
    
    private void OnDrawGizmos()
    {
	    if (gridNodeReferences == null)
		    return;
	    
	    for (int x = 0; x < width; x++)
	    {
		    for (int z = 0; z < length; z++)
		    {
			    if (gridNodeReferences[x, z].isBlocked)
			    {
				    Gizmos.color = Color.red;
				    Gizmos.DrawCube(new Vector3(x, 0, z), Vector3.one);
			    }
		    }
	    }
    }


}
