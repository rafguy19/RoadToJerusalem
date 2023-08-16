using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterZombieMovement : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PATROL,
        CHASE,
        POUNCE,
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
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
    private float idleTime;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 1;
        zombieAttack = GetComponentInChildren<ZombieAttack>();
        rb = GetComponentInParent<Rigidbody2D>();
        ChangeState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.IDLE)
        {
            Idle();
        }
        else if (currentState == State.PATROL)
        {
            Patrol();
        }
        else if (currentState == State.CHASE)
        {
            Chase();
        }
        else if (currentState == State.POUNCE)
        {
            Pounce();
        }
    }

    private void ChangeState(State next)
    {
        if (next == State.IDLE)
        {
            idleTime = 0.0f;
            GetComponent<HunterZombieAI>().enabled = false;
        }
        else if (next == State.PATROL)
        {
            GetComponent<HunterZombieAI>().enabled = true;
        }
        else if (next == State.CHASE)
        {
            GetComponent<HunterZombieAI>().enabled = true;
        }
        else if (next == State.POUNCE)
        {
            GetComponent<HunterZombieAI>().enabled = false;
        }
        currentState = next;
    }

    private void Idle()
    {
        idleTime += Time.deltaTime;

        if (idleTime >= 1.5f)
        {
            ChangeState(State.PATROL);
        }
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
            attackTimerCountdown = attackTimer;
            ChangeState(State.POUNCE);
        }
    }

    private void Pounce()
    {
        isAttacking = false;
        if (!isAttacking)
        {
            attackTimerCountdown -= Time.deltaTime;
            if (attackTimerCountdown <= 0)
            {
                zombieAttack.DealDamage();
                attackTimerCountdown = attackTimer;
                isAttacking = true;
            }
        }
        if (Vector3.Distance(transform.position, target.transform.position) > attackDist)
        {
            ChangeState(State.CHASE);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > 7.0f)
        {
            ChangeState(State.PATROL);
        }
    }
}
