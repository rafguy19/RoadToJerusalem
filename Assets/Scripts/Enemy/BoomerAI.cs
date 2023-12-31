using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class BoomerAI : MonoBehaviour
{
    public enum FLIP
    {
        LEFT,
        RIGHT,
    }

    public FLIP spriteFlip;
    int xSpriteScale;

    public Transform target;
    public float nextWaypointDistance = 0.1f;
    public Transform zombieGFX;

    private int EnemySpeed;

    private BoomerMovement boomerMove; 

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    //private float TrueSpeed;
    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if(spriteFlip == FLIP.LEFT)
        {
            xSpriteScale = 1;
        }
        else
        {
            xSpriteScale = -1;
        }
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        boomerMove = GetComponent<BoomerMovement>();
        //TrueSpeed = EnemySpeed[Random.Range(0, EnemySpeed.Length)];
        System.Random random = new System.Random();
        EnemySpeed = random.Next(200, 401);
        InvokeRepeating("UpdatePath", 0f, .5f);
        //Debug.Log(EnemySpeed);
    }

    void UpdatePath()
    {
        //if (boomerMove.currentState == BoomerMovement.State.PATROL)
        //{
        //    if(boomerMove.waypoints.Count != 0)
        //    {
        //        if (seeker.IsDone())
        //            seeker.StartPath(rb.position, boomerMove.waypoints[boomerMove.targetIndex].transform.position, OnPathComplete);
        //    }
        //}
        if (boomerMove.currentState == BoomerMovement.State.CHASE)
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

        if (direction.x >= 0.01f)
        {
            zombieGFX.localScale = new Vector3(-xSpriteScale, 1f, 1f);
        }
        else if (direction.x <= -0.01f)
        {
            zombieGFX.localScale = new Vector3(xSpriteScale, 1f, 1f);
        }
    }
}
