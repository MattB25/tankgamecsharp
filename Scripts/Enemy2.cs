using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour {

    Transform target;

    public float sightDistance;

    public Transform pathHolder;
    public float pathWaitTime = 0.3f;
    public float speed = 1.75f;
    public float turnSpeed = 90;

    public ShellController shell1;
    public float shellSpeed1;
    public Transform shellStart1;

    public float timeBetweenShots1;
    private float shotCounter1;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //Makes an array of all the positions of the path for moving around

        Vector3[] waypoints = new Vector3[pathHolder.childCount]; //counts how many children are in the array
        for (int i = 0; i < waypoints.Length; i++) //looks through each point in array
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z); //keeps tank even on the y axis
        }

        StartCoroutine(FollowPath(waypoints));
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < sightDistance) //if player is within sight distance, AI fires shells at a certain rate at the players position
        {
            //Debug.Log("enemy is within distance");
            var relativePos = target.position - transform.position;

            var rotation = Quaternion.LookRotation(relativePos);

            transform.rotation = rotation; // looks at the player once it is in sight

            shotCounter1 -= Time.deltaTime;
            if (shotCounter1 <= 0) //fires bullet and resets shotcounter if it goes below or equal to 0
            {
                shotCounter1 = timeBetweenShots1;
                ShellController newShell = Instantiate(shell1, shellStart1.position, shellStart1.rotation) as ShellController; //creates a new shell at the shell start position
                newShell.speed = shellSpeed1;
            }
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex]; 
        transform.LookAt(targetWaypoint); //so guard faces first waypoint when it spawns

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime); //moves the AI to it's target waypoint from current position
            if (transform.position == targetWaypoint) //if it reaches waypoint
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length; //moves onto the next point, when '(targetWaypointIndex + 1)' == '% waypoints.Length' makes it go back to 0
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(pathWaitTime); 
                yield return StartCoroutine(TurnToFace(targetWaypoint)); //waits while guard is rotating then starts coroutine
            }
            yield return null; 
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 directionToLookTarget = (lookTarget - transform.position); 
        float targetAngle = 90 - Mathf.Atan2(directionToLookTarget.z, directionToLookTarget.x) * Mathf.Rad2Deg; //makes new target 90 degrees away from where it is facing. '* Mathf.Rad2Deg' converts it from radians to degrees

        while (Mathf.DeltaAngle (transform.eulerAngles.y, targetAngle) > 0) //stops loop running when it is facing the target
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime); //goes from rotation on y axis to target angle
            transform.eulerAngles = Vector3.up * angle;
            yield return null; //makes sure it doesn't run in same frame
        }
    }

    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f); //draws spheres at waypoint positions
            Gizmos.DrawLine(previousPosition, waypoint.position); //puts lines in between them
            previousPosition = waypoint.position; //makes previous point the waypoint position
        }
        Gizmos.DrawLine(previousPosition, startPosition); //draws a line from the last 
    }
}
