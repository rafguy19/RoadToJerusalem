using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerMovement : MonoBehaviour
{
    public enum State
    {
        PATROL,
        CHASE,
        EXPLODE,
    }
    [SerializeField]
    public State currentState;
    [SerializeField]
    public List<GameObject> waypoints = new List<GameObject>();
    private float attackDist = 1.87f;
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private ZombieAttack zombieAttack;
    public bool isAttacking = false;


    public Animator animator;
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<BoomerAI>().target;
        zombieAttack = GetComponentInChildren<ZombieAttack>();
        rb = GetComponentInParent<Rigidbody2D>();
        ChangeState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.PATROL:
                Patrol();
                break;
            case State.CHASE:
                Chase();
                break;
            case State.EXPLODE:
                Explode();
                break;
        }
    }

    private void ChangeState(State next)
    {

        if (next == State.PATROL)
        {
            GetComponent<BoomerAI>().enabled = true;
        }
        else if (next == State.CHASE)
        {
            GetComponent<BoomerAI>().enabled = true;
        }
        else if (next == State.EXPLODE)
        {
            GetComponent<BoomerAI>().enabled = false;
        }
        currentState = next;
    }

    private void Patrol()
    {
        if (Vector3.Distance(waypoints[targetIndex].transform.position, transform.position) <= 0.5f)
        {
            targetIndex++;
            targetIndex %= waypoints.Count;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= 7.0f)
        {
    
            ChangeState(State.CHASE);
        }
    }

    private void Chase()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 7.0f)
        {

            ChangeState(State.PATROL);
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= attackDist)
        {
            ChangeState(State.EXPLODE);
        }
    }

    private void Explode()
    {
        animator.SetTrigger("attack");

    }
}
