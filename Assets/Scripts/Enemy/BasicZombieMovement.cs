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
        DEATH,
    }
    [SerializeField]
    public State currentState;
    [SerializeField]
    public List<GameObject> waypoints = new List<GameObject>();
    private float attackDist = 1;
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    public bool isAttacking = false;
    public float attackTimer;
    private float attackTimerCountdown;

    public Animator animator;
    private Transform target;

    BasicZombieAttack basicZombieAttack;
    // Start is called before the first frame update
    void Start()
    {
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();
        target = GetComponent<BasicZombieAI>().target;
        rb = GetComponentInParent<Rigidbody2D>();
        ChangeState(currentState);
    }

    // Update is called once per frame
    void Update()
    {

            switch (currentState)
            {
                case State.PATROL:
                    animator.SetBool("isWalking", true);
                    Patrol();
                    break;
                case State.CHASE:
                    animator.SetBool("isWalking", true);
                    Chase();
                    break;
                case State.ATTACK:
                    animator.SetBool("isWalking", false);
                    Attack();
                    break;
                case State.DEATH:
                    rb.velocity = Vector2.zero;
                    break;
            }
        if (basicZombieAttack.isDead == true)
        {
            currentState = State.DEATH;
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
            attackTimerCountdown = 0;
            ChangeState(State.ATTACK);
        }
    }

    private void Attack()
    {
        attackTimerCountdown -= Time.deltaTime;
        //Debug.Log(attackTimerCountdown);
        if (attackTimerCountdown <= 0)
        {
            animator.SetTrigger("attack");
            attackTimerCountdown = attackTimer;
            isAttacking = true;
        }
        if (Vector3.Distance(transform.position, target.transform.position) > attackDist && isAttacking == false)
        {
            ChangeState(State.CHASE);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > 7.0f && isAttacking == false)
        {
            ChangeState(State.PATROL);
        }
    }
}
