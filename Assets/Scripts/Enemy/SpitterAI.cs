using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpitterAI : MonoBehaviour
{
    public enum FLIP
    {
        LEFT,
        RIGHT,
    }

    public FLIP spriteFlip;
    float xSpriteScale;

    public Transform target;
    public float nextWaypointDistance = 3f;
    public Transform zombieGFX;

    private int EnemySpeed;

    private SpitterMovement spitterMove;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    //private float TrueSpeed;
    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (spriteFlip == FLIP.LEFT)
        {
            xSpriteScale = 1;
        }
        else
        {
            xSpriteScale = 1;
        }
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        spitterMove = GetComponent<SpitterMovement>();
        System.Random random = new System.Random();
        EnemySpeed = 100;
        InvokeRepeating("UpdatePath", 0f, .5f);

    }

    void UpdatePath()
    {
        //if (spitterMove.waypoints.Count != 0)
        //{
        //    if (spitterMove.currentState == SpitterMovement.State.PATROL)
        //    {
        //        if (seeker.IsDone())
        //            seeker.StartPath(rb.position, spitterMove.waypoints[spitterMove.targetIndex].transform.position, OnPathComplete);
        //    }
        //}

        if (spitterMove.currentState == SpitterMovement.State.CHASE)
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
            zombieGFX.localScale = new Vector3(xSpriteScale, 1, 1);
        }
        else if (force.x <= -0.01f)
        {
            zombieGFX.localScale = new Vector3(-xSpriteScale, 1, 1);
        }
    }
}
