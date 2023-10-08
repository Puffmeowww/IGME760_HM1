using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PF_PlayerCharacter : MonoBehaviour
{

    //Movement Speed
    public float speed = 10.0f;
    //Max Rotation Speed
    public float maxRotationSpeed = 360f;
    int targetIndex;
    Vector3[] path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 PFtargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PathRequestManager.RequestPath(transform.position, PFtargetPosition, OnPathFound);
        }

    }

    //Callback method that is called when a path is found
    void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {  
        if (pathSuccessful)
        {
            // Move the player along the path
            path = newPath;
            //Stop current coroutine
            StopCoroutine("FollowPath");
            //Start a new coroutine
            StartCoroutine(FollowPath(path));

            //print("PlayerCharacter find path successfully");
        }
        else
        {
            //print("PlayerCharacter cannot find path");
        }
    }

    IEnumerator FollowPath(Vector3[] path)
    {
        /*        foreach (Vector3 waypoint in path)
                {
                    while (transform.position != waypoint)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, waypoint, speed * Time.deltaTime);

                        FaceTarget(waypoint- transform.position);
                        yield return null;
                    }

                }*/
        if (path != null && path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
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

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
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
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        }
    }

}
