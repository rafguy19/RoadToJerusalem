using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombieMovement : MonoBehaviour
{
    public enum State
    {
        PATROL,
        CHASE,
        ATTACK,
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
        if (currentState == State.PATROL)
        {
            Patrol();
        }
        else if (currentState == State.CHASE)
        {
            Chase();
        }
        else if (currentState == State.ATTACK)
        {
            Attack();
        }
    }

    private void ChangeState(State next)
    {

        if (next == State.PATROL)
        {
            GetComponent<BasicZombieAI>().enabled = true;
        }
        else if (next == State.CHASE)
        {
            GetComponent<BasicZombieAI>().enabled = true;
        }
        else if (next == State.ATTACK)
        {
            GetComponent<BasicZombieAI>().enabled = false;
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
            attackTimerCountdown = attackTimer;
            ChangeState(State.ATTACK);
        }
    }

    private void Attack()
    {
        isAttacking = false;
        if (!isAttacking)
        {
            attackTimerCountdown -= Time.deltaTime;
            Debug.Log(attackTimerCountdown);
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
