using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode
{

    public bool walkable;
    //Node's world pos
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public ANode parent;

    //constructure
    public ANode(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost
    {
        get{
            return gCost + hCost;
        }
    }

}
