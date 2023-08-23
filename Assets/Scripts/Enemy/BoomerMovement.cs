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
        DEATH,
    }
    [SerializeField]
    public State currentState;
    public List<GameObject> waypoints = new List<GameObject>();
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private BasicZombieAttack basicZombieAttack;

    public Transform detectionCircle;
    public float detectionRange;

    public Transform attackCircle;
    public float attackRange;

    public float explosionDelay;

    public Animator animator;
    private Transform target;

    private AudioSource audioSource;
    public AudioClip boomerDelaySound;
    bool delaySoundPlayed = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = GetComponent<BoomerAI>().target;
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();
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
            case State.EXPLODE:
                animator.SetBool("isWalking", false);
                if(delaySoundPlayed == false)
                {
                    audioSource.PlayOneShot(boomerDelaySound);
                    delaySoundPlayed= true;
                }
                explosionDelay -= Time.deltaTime;
                rb.velocity= Vector3.zero;
                if (explosionDelay < 0)
                {
                    Explode();
                }


                break;
            case State.DEATH:

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
       
        if(waypoints.Count != 0)
        {
            if (Vector3.Distance(waypoints[targetIndex].transform.position, transform.position) <= 0.5f)
            {
                targetIndex++;
                targetIndex %= waypoints.Count;
            }
        }



        if (Vector3.Distance(transform.position, target.transform.position) <= detectionRange)
        {

            ChangeState(State.CHASE);
        }
    }

    private void Chase()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {

                currentState= State.EXPLODE;
     
        }
    }

    private void Explode()
    {
 
        animator.SetTrigger("Exploding");
        currentState = State.DEATH;
    }
    private void OnDrawGizmosSelected()
    {
        if (detectionCircle == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(detectionCircle.position, detectionRange);

        Gizmos.DrawWireSphere(attackCircle.position, attackRange);
    }
}
