using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class BasicZombieAI : MonoBehaviour
{
    public Transform target;
    public float nextWaypointDistance = 3f;
    public Transform zombieGFX;

    private int EnemySpeed;

    private BasicZombieMovement basicZombieMove; 

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    //private float TrueSpeed;
    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        basicZombieMove = GetComponent<BasicZombieMovement>();
        //TrueSpeed = EnemySpeed[Random.Range(0, EnemySpeed.Length)];
        System.Random random = new System.Random();
        EnemySpeed = random.Next(200, 401);
        InvokeRepeating("UpdatePath", 0f, .5f);
        Debug.Log(EnemySpeed);
    }

    void UpdatePath()
    {
        if (basicZombieMove.currentState == BasicZombieMovement.State.PATROL)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, basicZombieMove.waypoints[basicZombieMove.targetIndex].transform.position, OnPathComplete);
        }
        else if (basicZombieMove.currentState == BasicZombieMovement.State.CHASE)
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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * EnemySpeed * Time.deltaTime;

        rb.AddForce(force);

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
