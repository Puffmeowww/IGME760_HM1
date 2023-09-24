using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //Varaibles
    //public float moveSpeed = 10.0f;
    public float targetRadius = 0.5f;
    private Vector2 targetPosition;


    //Dynamic movement variables
    public Rigidbody2D rb;
    public float maxSpeed = 12.0f;
    public float maxAcceleration = 5.0f;

    public float slowRadius = 2.0f;
    public float dt = 0.1f;

    private Vector2 currentVelocity = default;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //When the player click on a new position
        if (Input.GetMouseButtonDown(0))
        {
            //Get player's mouse position
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


        //Get character position, direction and velocity
        Vector2 characterPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 direction = targetPosition - characterPosition;
        //Vector2 velocity = direction.normalized * moveSpeed;

        //Get distance from the target
        float distance = direction.magnitude;

        if (distance <= targetRadius)
        {
            return;
        }

        float targetSpeed = 0;
        Vector2 targetVelocity = default;

        //Check if the character is in slow radius
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * (distance / slowRadius);
        }


        //Set target velocity
        targetVelocity = direction.normalized * targetSpeed;

        //Set accelaration
        Vector2 acceleration = targetVelocity - rb.velocity;
        acceleration /= dt;

        if(acceleration.magnitude > maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        currentVelocity = new Vector2(
            currentVelocity.x + acceleration.x * Time.deltaTime,
            currentVelocity.y + acceleration.y * Time.deltaTime);

        FaceTarget(currentVelocity);

        rb.velocity = currentVelocity;

        //Movement
        if ((targetPosition - characterPosition).magnitude >= targetRadius)
        {

            //Make the character face the target


            //Change the position
            float newX = transform.position.x + rb.velocity.x * Time.deltaTime;
            float newY = transform.position.y + rb.velocity.y * Time.deltaTime;

            transform.position = new Vector2(newX, newY);
        }


    }



    public void FaceTarget(Vector3 velocity)
    {
        if(velocity.magnitude > 0)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }


}