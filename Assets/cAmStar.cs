using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cAmStar : MonoBehaviour
{
	public Map map;

	public List<Node> open;
	public List<Node> closed;

	public Node current;

	public List<Node> finalPath;


    // Start is called before the first frame update
    void Start()
    {
		// Neighbours recalc
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				
			}
		}
    }

}
