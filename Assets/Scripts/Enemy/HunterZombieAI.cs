using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HunterZombieAI : MonoBehaviour
{
    public Transform target;
    public float nextWaypointDistance = 3f;
    public Transform zombieGFX;

    public int EnemySpeed;

    private HunterZombieMovement hunterZombieMovement;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        hunterZombieMovement = GetComponent<HunterZombieMovement>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (hunterZombieMovement.waypoints.Count != 0)
        {
            if (hunterZombieMovement.currentState == HunterZombieMovement.State.PATROL)
            {
                if (seeker.IsDone())
                    seeker.StartPath(rb.position, hunterZombieMovement.waypoints[hunterZombieMovement.targetIndex].transform.position, OnPathComplete);
            }
        }
        else if (hunterZombieMovement.currentState == HunterZombieMovement.State.CHASE)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }


    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if(hunterZombieMovement.currentState != HunterZombieMovement.State.POUNCE || hunterZombieMovement.currentState != HunterZombieMovement.State.ATTACK)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * EnemySpeed * Time.deltaTime;

            rb.velocity = force;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if (force.x >= 0.01f)
            {
                zombieGFX.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (force.x <= -0.01f)
            {
                zombieGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        
    }
}
