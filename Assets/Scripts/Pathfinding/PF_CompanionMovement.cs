using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_CompanionMovement : MonoBehaviour
{
    //Movement Speed
    public float speed = 10.0f;
    //Max Rotation Speed
    public float maxRotationSpeed = 360f;
    public float stopRadius = 3.0f;
    public Transform player;
    Coroutine moveCoroutine;

    int targetIndex;

    //The path the companion needs to follow
    Vector3[] path;

    // Update is called once per frame
    void Update()
    {
        //Request a path to the player's current location
        PathRequestManager.RequestPath(transform.position, player.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {

        //Check if the path is found
        if (pathSuccessful)
        {
            path = newPath;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            //Start a new coroutine
            moveCoroutine = StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path != null && path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                //Check if the companion is too close to the player
                if (Vector3.Distance(transform.position, player.position) <= stopRadius)
                {
                    StopCoroutine("FollowPath");
                    targetIndex = 0;
                    currentWaypoint = path[targetIndex];
                    break;
                }

                //If get the waypoint, set the current waypoint to the next waypoint
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        targetIndex = 0;
                        currentWaypoint = path[targetIndex];
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                //Move towards the current waypoint (next destination)
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                //Face the target
                FaceTarget(currentWaypoint - transform.position);
                yield return null;
            }

        }
     }

    // Face target function
    void FaceTarget(Vector3 velocity)
    {
        if (velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        }
    }

}
