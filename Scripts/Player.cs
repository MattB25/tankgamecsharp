using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))] //Makes sure the scripts are linked to the same object
public class Player : MonoBehaviour
{
    public float moveSpeed = 2; 
    Camera cam;
    PlayerController cont;

    public TurretController theTurret;
	void Start ()
    {
        cont = GetComponent<PlayerController>(); //Assumes the player controller script attached to the same object as this script
        cam = Camera.main;
	}

	void Update ()
    {
        //MOVEMENT
        
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); //Gets the movement input for the player, 0 leaves the y-axis out, Raw stops the tank moving as soon as you let go of the key
        Vector3 moveVelocity = moveInput.normalized * moveSpeed; //normalised to just get direction
        cont.Move(moveVelocity); 

        //LOOKING AT MOUSE

        Ray ray = cam.ScreenPointToRay(Input.mousePosition); //points a ray through the mouse position from the camera
        Plane groundPlane = new Plane(Vector3.up, transform.position); //imaginary ground plane to show what level the tank looks at
        float rayDistance; 

        if(groundPlane.Raycast(ray,out rayDistance)) //raycast takes in a ray and gives out a variable, if camera intersects with groundplane then the length from the camera to the intersect is known
        {
            Vector3 intersect = ray.GetPoint(rayDistance);
            cont.LookAt(intersect); 
        }

        //FIRING

        if (Input.GetMouseButtonDown(0)) //if mouse button is held down it is firing
        {
            theTurret.isFiring = true; 
        }
        if (Input.GetMouseButtonUp(0)) //if mouse button is held down it is not firing
        {
            theTurret.isFiring = false;
        }

    }
}
