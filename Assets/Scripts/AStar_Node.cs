using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANode
{

    public bool walkable;
    //Node's world pos
    public Vector3 worldPosition;


    //constructure
    public ANode(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
    }

}
