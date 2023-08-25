using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class BasicZombieAI : MonoBehaviour
{
    public enum FLIP
    {
        LEFT,
        RIGHT,
    }

    public FLIP spriteFlip;
    int xSpriteScale;

    public Transform target;
    public float nextWaypointDistance = 3f;
    public Transform zombieGFX;
    public LayerMask blockage;
    private int EnemySpeed;
    public float sideForce;
    private BasicZombieMovement basicZombieMove; 

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    private Vector2 direction;
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
        basicZombieMove = GetComponent<BasicZombieMovement>();
        System.Random random = new System.Random();
        EnemySpeed = random.Next(100, 501);
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (basicZombieMove.waypoints.Count != 0)
        {
            if (basicZombieMove.currentState == BasicZombieMovement.State.PATROL)
            {
                if (seeker.IsDone())
                    seeker.StartPath(rb.position, basicZombieMove.waypoints[basicZombieMove.targetIndex].transform.position, OnPathComplete);
            }
        }

        if (basicZombieMove.currentState == BasicZombieMovement.State.CHASE)
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

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((blockage.value & (1 << other.gameObject.layer)) != 0)
        {
            Vector2 sideForceDirection = new Vector2(-other.contacts[0].normal.y, other.contacts[0].normal.x).normalized;
            rb.AddForce(sideForceDirection * sideForce, ForceMode2D.Force);
        }
    }
}
