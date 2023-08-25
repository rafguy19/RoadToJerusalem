using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterMovement : MonoBehaviour
{
    public enum State
    {
        PATROL,
        CHASE,
        SPIT
    }
    [SerializeField]
    public State currentState;
    [SerializeField]
    public List<GameObject> waypoints = new List<GameObject>();
    private float attackDist = 1.87f;
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private BasicZombieAttack basicZombieAttack;
    private GameObject spitter;
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
    private int prevHealth;
    private Animator ar;
    private bool dead;
    public Transform target;
    private ZombieSpit zombieSpit;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 1;
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();
        spitter = GameObject.FindGameObjectWithTag("Spitter");
        zombieSpit = GetComponent<ZombieSpit>();
        rb = GetComponentInParent<Rigidbody2D>();
        ar = GetComponentInChildren<Animator>();
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
        else if (currentState == State.SPIT)
        {
            rb.velocity = Vector2.zero;
            Spit();
        }
        if(basicZombieAttack.enemyCurrentHealth <= 0 && dead == false)
        {
            dead = true;
            Death();
        }
    }

    private void Death()
    {
        ar.SetTrigger("Death");
    }

    private void ChangeState(State next)
    {
        if (next == State.PATROL)
        {
            ar.SetBool("Moving", true);
            GetComponent<SpitterAI>().enabled = true;
        }
        else if (next == State.CHASE)
        {
            ar.SetBool("Moving", true);
            GetComponent<SpitterAI>().enabled = true;
        }
        else if (next == State.SPIT)
        {
            ar.SetBool("Moving", false);
            ar.SetTrigger("Attack");
            GetComponent<SpitterAI>().enabled = false;
        }
        currentState = next;
    }

    private void Patrol()
    {
        if (waypoints.Count != 0)
        {
            if (Vector3.Distance(waypoints[targetIndex].transform.position, transform.position) <= 0.5f)
            {
                targetIndex++;
                targetIndex %= waypoints.Count;
            }
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= 15.0f)
        {
            ChangeState(State.CHASE);
        }
    }

    private void Chase()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 15.0f)
        {

            ChangeState(State.PATROL);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) <= 6.0f)
        {
            zombieSpit.enterSpit = false;
            ChangeState(State.SPIT);
        }
    }

    private void Spit()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 10f)
        {
            ChangeState(State.CHASE);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > 12.5f)
        {
            ChangeState(State.PATROL);
        }
    }
}
