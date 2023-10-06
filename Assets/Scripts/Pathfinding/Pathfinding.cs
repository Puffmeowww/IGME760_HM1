using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;

    AGrid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<AGrid>();
    }  

    IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        ANode startNode = grid.NodeFromWorldPoint(startPosition);
        ANode targetNode = grid.NodeFromWorldPoint(targetPosition);

        if (startNode.walkable && targetNode.walkable)
        {

            Heap<ANode> openSet = new Heap<ANode>(grid.MaxSize);
            HashSet<ANode> closedSet = new HashSet<ANode>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                ANode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                //Find the shortest path
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
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
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }

        }
        yield return null;

        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    int GetDistance(ANode nodeA, ANode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    Vector3[] RetracePath(ANode startNode, ANode endNode)
    {
        List<ANode> path = new List<ANode>();
        ANode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path,startNode);

        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<ANode> path, ANode startNode)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i - 1].worldPosition); //Changed from path[i] to path[i-1]
            }
            directionOld = directionNew;
            if (i == path.Count - 1 && directionOld != new Vector2(path[i].gridX, path[i].gridY) - new Vector2(startNode.gridX, startNode.gridY))
                waypoints.Add(path[path.Count - 1].worldPosition);
        }
        return waypoints.ToArray();

    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }


}
