using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    //Varaibles
    public float moveSpeed = 10.0f;
    public float targetRadius = 0.5f;
    private Vector2 targetPosition;


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
        Vector2 velocity = direction.normalized * moveSpeed;

        FaceTarget(velocity);


        //Movement
        if ((targetPosition - characterPosition).magnitude >= targetRadius)
        {

            //Make the character face the target


            //Change the position
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
