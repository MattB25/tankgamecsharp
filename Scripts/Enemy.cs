using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Transform target;

    public float speed;
    public float retreatDistance;
    public float sightDistance;

    public ShellController shell1;
    public float shellSpeed1;
    public Transform shellStart1;

    public float timeBetweenShots1;
    private float shotCounter1;

    //public float radius = 20f;
    //private Vector3 wanderPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform; //Makes the player the target and therefore knows where to find it

        StartCoroutine(UpdatePath());

        //wanderPoint = RandomWanderPoint(); //gives random wander point at start
    }


    void Update()
    {
        //ATTEMPT AT FINITE STATE 

        if (Vector3.Distance(transform.position, target.position) > sightDistance) //if player is out of sight enemy should wander around
        {
            print("This is where the AI should wander");
            //Wander();
        }

        if (Vector3.Distance(transform.position, target.position) < retreatDistance) //if playertank distance is less than retreat distance then move away from player
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }

        //SHOOTING AT PLAYER

        if (Vector3.Distance(transform.position, target.transform.position) < sightDistance) //if player is within sight distance, AI fires shells at a certain rate at the players position
        {
            //Debug.Log("enemy is within distance");
            var relativePos = target.position - transform.position;

            var rotation = Quaternion.LookRotation(relativePos);

            transform.rotation = rotation; //looks at player once in sight

            shotCounter1 -= Time.deltaTime;
            if (shotCounter1 <= 0) //fires bullet and resets shotcounter if it goes below or equal to 0
            {
                shotCounter1 = timeBetweenShots1;
                ShellController newShell = Instantiate(shell1, shellStart1.position, shellStart1.rotation) as ShellController; //creates a new shell at the shell start position
                newShell.speed = shellSpeed1;
            }
        }
    }

    IEnumerator UpdatePath() //goes through this loop and repeats it every _ seconds depending on the refresh rate, better performance than refreshing every frame
    {
        float refreshRate = 0.25f;
        while (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
            agent.SetDestination(targetPosition);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    //public Vector3 RandomWanderPoint()
    //{
    //    Vector3 randomPoint = Random.insideUnitSphere * radius + transform.position; //calculates a random point inside a sphere at the enemy tank
    //    NavMeshHit navHit;
    //    NavMesh.SamplePosition(randomPoint, out navHit, radius, -1); //gets random point in sphere and gets closest point on navmesh
    //    return new Vector3(navHit.position.x, transform.position.y, navHit.position.z); //puts the positions randomly on the x and z axis
    //}

    //public void Wander()
    //{
    //    if (Vector3.Distance(transform.position, wanderPoint) < 2f) //checks the distance between the wander point and tank
    //    {
    //        wanderPoint = RandomWanderPoint(); //make a new wander point if one has been reached
    //    }
    //    else
    //    {
    //        pathfinder.SetDestination(wanderPoint); //find new wander point if it hasnt reached wander point
    //    }

    //}
}
