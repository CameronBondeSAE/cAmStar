using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cAmStar : MonoBehaviour
{
    public Map map;

    public List<Node> open;
    public List<Node> closed;
    public List<Node> finalPath;

    public Node current;


    public GameObject startPrefab;
    public GameObject targetPrefab;
    public GameObject pathCubePrefab;
    private Vector2Int start;
    private Vector2Int target;
    public float visualiseSpeed = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
//	    open = new List<Node>();
//	    closed = new List<Node>();
//	    finalPath = new List<Node>();

        RandomlyPositionStartAndTarget();

        StartCoroutine(FindPath());
    }

    public IEnumerator FindPath()
    {
        current = map.grid[start.x, start.y];
        open.Add(current); // Initial starting point

        // Loop until end found
        while (open.Count > 0)
        {
            FindLowestFCost();
            // HACK TODO DEBUG
            current.debugGO.GetComponentInChildren<Renderer>().material.color = Color.green;

            int xCheck = 0;
            int yCheck = 0;

            // Neighbours recalc
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    // Same as current so bail
                    if (x == 0 && y == 0)
                        continue;

                    xCheck = current.position.x + x;
                    yCheck = current.position.y + y;

                    // Bail if out of bounds or the current node, or in the closed list
                    if (xCheck < 0 || yCheck < 0 || xCheck >= map.size.x || yCheck >= map.size.y)
                        continue;


                    // Reached end
                    if (xCheck == target.x && yCheck == target.y)
                    {
                        Debug.Log("FOUND TARGET");
                        while (current.parent != null)
                        {
                            Instantiate(pathCubePrefab,
                                new Vector3(current.parent.position.x, 0, current.parent.position.y),
                                Quaternion.identity);
                            current = current.parent;
                            yield return new WaitForSeconds(visualiseSpeed);
                        }

                        yield break;
                    }


                    Node nodeToCheck = map.grid[xCheck, yCheck];
                    if (closed.Contains(nodeToCheck) || nodeToCheck.isBlocked)
                        continue;

                    // Note: Multiply by ten to maintain ints for distances
                    nodeToCheck.hCost = 10 * (int) Vector2.Distance(
                                            new Vector2((float) current.position.x, (float) current.position.y),
                                            new Vector2((float) target.x, (float) target.y));
                    nodeToCheck.gCost = current.gCost + 10 * (int) Vector2.Distance(
                                            new Vector2((float) current.position.x, (float) current.position.y),
                                            new Vector2((float) nodeToCheck.position.x,
                                                (float) nodeToCheck.position.y));

                    // fCost
                    nodeToCheck.fCost = nodeToCheck.hCost + nodeToCheck.gCost;

                    // Debug
                    if (nodeToCheck.debugGO.GetComponentInChildren<TextMesh>() != null)
                        nodeToCheck.debugGO.GetComponentInChildren<TextMesh>().text =
                            nodeToCheck.gCost + ":" + nodeToCheck.hCost + "\n" + nodeToCheck.fCost;

                    nodeToCheck.parent = current;

                    Debug.DrawLine(new Vector3(nodeToCheck.position.x, 0, nodeToCheck.position.y),
                        new Vector3(nodeToCheck.position.x, 10f, nodeToCheck.position.y), Color.magenta, 0.1f, false);

                    open.Add(nodeToCheck);

                    yield return new WaitForSeconds(visualiseSpeed / 10f);
                }
            }

            // Done with this node
            open.Remove(current);
            closed.Add(current);
            // HACK TODO DEBUG
            current.debugGO.GetComponentInChildren<Renderer>().material.color = Color.red;

            yield return new WaitForSeconds(visualiseSpeed);
        }
    }

    private Node FindLowestFCost()
    {
//        int lowest = open.Min(Node => Node.fCost);

        // Find next lowest fCost
        int lowestFCost = int.MaxValue;
        Node lowestFCostNode = null;

        foreach (Node node in open)
        {
            if (node.fCost < lowestFCost)
            {
                lowestFCost = node.fCost;
                lowestFCostNode = node;
            }
        }

        return lowestFCostNode;
    }

    private void RandomlyPositionStartAndTarget()
    {
        start = map.FindUnblockedSpace();
        Instantiate(startPrefab, new Vector3(start.x, 0, start.y), Quaternion.identity);
        target = map.FindUnblockedSpace();
        Instantiate(targetPrefab, new Vector3(target.x, 0, target.y), Quaternion.identity);
    }


//    private void OnDrawGizmos()
//    {
//        for (int x = 0; x < map.size.x; x++)
//        {
//            for (int y = 0; y < map.size.y; y++)
//            {
//                if (map.grid != null)
//                {
//                    if (map.grid[x, y].isBlocked)
//                    {
//                        Gizmos.color = Color.red;
//                        Gizmos.DrawCube(new Vector3(x, -0.45f, y), Vector3.one);
//                    }
//
//                    if (open.Contains(map.grid[x, y]))
//                    {
//                        Gizmos.color = Color.green;
//                        Gizmos.DrawCube(new Vector3(x, -0.45f, y), Vector3.one);
//                    }
//
//                    if (closed.Contains(map.grid[x, y]))
//                    {
//                        Gizmos.color = Color.gray;
//                        Gizmos.DrawCube(new Vector3(x, -0.45f, y), Vector3.one);
//                    }
//                }
//            }
//        }


//		// Scan the real world starting at 0,0,0 (to be able to place the grid add transform.position)
//		for (int x = 0; x < size.x; x++)
//		{
//			for (int y = 0; y < size.y; y++)
//			{
//				if (Physics.CheckBox(transform.position + new Vector3(x, 0, y), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity))
//				{
//					// Something is there
//					grid[x, y].isBlocked = true;
//					Gizmos.color = Color.red;
//					Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), Vector3.one);
//				}
//			}
//		}
//    }
}