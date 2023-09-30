using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : MonoBehaviour
{

    public GameObject targetObject;
    public float moveSpeed = 10.0f;
    public float targetRadius = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 direction = targetObject.transform.position - transform.position;
        Vector2 velocity = direction.normalized * moveSpeed;

        Vector2 characterPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 targetPosition = targetObject.transform.position;

        FaceTarget(velocity);

        if ((targetPosition - characterPosition).magnitude >= targetRadius)
        {
            float newX = transform.position.x + velocity.x * Time.deltaTime;
            float newY = transform.position.y + velocity.y * Time.deltaTime;

            transform.position = new Vector2(newX, newY);
        }


    }


    public void FaceTarget(Vector3 velocity)
    {
        if (velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }



}
