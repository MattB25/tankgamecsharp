using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour {

    public LayerMask collisionMask; //determines which layers the object will collide with
    public float speed;
	
	void Update ()
    {
        float moveDistance = speed * Time.deltaTime; 
        CheckCollisions(moveDistance); 
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance)
    {
        //IF SHELL HITS ANYTHING

        Ray ray = new Ray(transform.position, transform.forward); //creates a ray to check for collisions
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))  //if it collides with something then it destroys shell
        {
         GameObject.Destroy(gameObject); 
        }

        //IF SHELL HITS PLAYER/ENEMY

        if(Physics.Raycast(ray, out hit, 0.1f))
        {
            //print(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Player") //if the shell hits an enemy or the player then it calls the remove health method in the health class
            {
                hit.collider.gameObject.GetComponent<Health>().RemoveHealth();
            }
        }
    }
}
