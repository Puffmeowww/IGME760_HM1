using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    //Call unwalkable mask
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    //how much space each node covers
    public float nodeRadius;
    //2 Dimension grid
    ANode[,] grid;

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x, gridWorldSize.y));
        if (grid != null)
        {
            foreach (ANode n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.yellow : Color.red;
                //Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter - .1f));
            }
        }
    }

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    // Start is called before the first frame update
    void Start()
    {
        //Calculate numbers of grids
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid()
    {
        //Create grid based on the grid size
        grid = new ANode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
               //Check Collision
/*                if(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask))
                {
                    Debug.Log("collision");
                }*/
                //bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableMask));
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new ANode(walkable, worldPoint);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

}
