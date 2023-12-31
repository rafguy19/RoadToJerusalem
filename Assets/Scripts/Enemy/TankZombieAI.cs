using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TankZombieAI : MonoBehaviour
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
    public LayerMask blockage;
    public int EnemySpeed;
    public float sideForce;
    private TankZombieMovement tankZombieMovement;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteFlip == FLIP.LEFT)
        {
            xSpriteScale = 2.5f;
        }
        else
        {
            xSpriteScale = -2.5f;
        }
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        tankZombieMovement = GetComponent<TankZombieMovement>();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (tankZombieMovement.waypoints.Count != 0)
        {
            if (tankZombieMovement.currentState == TankZombieMovement.State.PATROL)
            {
                if (seeker.IsDone())
                    seeker.StartPath(rb.position, tankZombieMovement.waypoints[tankZombieMovement.targetIndex].transform.position, OnPathComplete);
            }
        }     
        else if (tankZombieMovement.currentState == TankZombieMovement.State.CHASE)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
        else if (tankZombieMovement.currentState == TankZombieMovement.State.GUARD)
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
            zombieGFX.localScale = new Vector3(-xSpriteScale, 2.5f, 2.5f);
        }
        else if (force.x <= -0.01f)
        {
            zombieGFX.localScale = new Vector3(xSpriteScale, 2.5f, 2.5f);
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
