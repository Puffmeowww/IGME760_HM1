using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    public Transform seeker, target;


    AGrid grid;

    void Awake()
    {
        grid = GetComponent<AGrid>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        ANode startNode = grid.NodeFromWorldPoint(startPosition);
        ANode targetNode = grid.NodeFromWorldPoint(targetPosition);

        List<ANode> openSet = new List<ANode>();
        HashSet<ANode> closedSet = new HashSet<ANode>();

        openSet.Add(startNode);

        while(openSet.Count >0)
        {
            ANode currentNode = openSet[0];

            //find smallest cost in the openlist, and set currentnode to that smallest cost node
            for(int i=0; i<openSet.Count;i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }


            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //Find the shortest path
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }


            //Check each neighbour
            foreach (ANode neighbour in grid.GetNeighbours(currentNode))
            {
                //Discard unwalkable and closedlist neighbour
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                //Check if current node should be this neighbour's parent node
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }

            }

        }

    }



    int GetDistance(ANode nodeA, ANode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }



    void RetracePath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        grid.path = path;
    }












}
