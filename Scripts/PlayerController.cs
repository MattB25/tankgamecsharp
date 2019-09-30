using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}

    public void Move (Vector3 velocity1)
    {
        velocity = velocity1; //controls velocity of player using velocity1
    }

    public void LookAt (Vector3 lookPoint)
    {
        transform.LookAt(lookPoint); //Player looks at the intersect point
    }

    public void FixedUpdate() //Executes multiple times a frame
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); //gives rigid boy a new position, delta time is the time in between the fixed update
    }
}
