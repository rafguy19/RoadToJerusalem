using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerMovement : MonoBehaviour
{
    public enum State
    {
        IDLE,
        //PATROL,
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
    public AudioClip boomerGrunt1;
    public AudioClip boomerGrunt2;
    bool delaySoundPlayed = false;

    bool gurnting;
    float gurntingDelay = 3;
    // Start is called before the first frame update
    void Start()
    {
        gurnting = false;
        audioSource = GetComponent<AudioSource>();
        target = GetComponent<BoomerAI>().target;
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();
        rb = GetComponentInParent<Rigidbody2D>();
        ChangeState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        //gruning sound
        if(gurnting == true)
        {
            gurntingDelay -= Time.deltaTime;
            if(gurntingDelay <= 0)
            {
                int randomGrunt = Random.Range(1, 3);
                switch (randomGrunt)
                {
                    case 1:
                        audioSource.PlayOneShot(boomerGrunt1);
                        break;
                    case 2:
                        audioSource.PlayOneShot(boomerGrunt2);
                        break;
                }
                gurntingDelay = Random.Range(1,4);
            }
        }
        switch (currentState)
        {
            case State.IDLE:
                gurnting = true;
                animator.SetBool("isWalking", false);
                Idle();
                break;
            //case State.PATROL:
            //    gurnting = true;
            //    animator.SetBool("isWalking", true);
            //    Patrol();
            //    break;
            case State.CHASE:
                gurnting = true;
                animator.SetBool("isWalking", true);
                Chase();
                break;
            case State.EXPLODE:
                gurnting = false;
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

        //if (next == State.PATROL)
        //{
        //    GetComponent<BoomerAI>().enabled = true;
        //}
        if (next == State.CHASE)
        {
            GetComponent<BoomerAI>().enabled = true;
        }
        else if (next == State.EXPLODE)
        {
            GetComponent<BoomerAI>().enabled = false;
        }
        currentState = next;
    }


    private void Idle()
    {


        if (Vector3.Distance(transform.position, target.transform.position) <= detectionRange)
        {

            ChangeState(State.CHASE);
        }
    }

    //private void Patrol()
    //{
       
    //    if(waypoints.Count != 0)
    //    {
    //        if (Vector3.Distance(waypoints[targetIndex].transform.position, transform.position) <= 0.5f)
    //        {
    //            targetIndex++;
    //            targetIndex %= waypoints.Count;
    //        }
    //    }



    //    if (Vector3.Distance(transform.position, target.transform.position) <= detectionRange)
    //    {

    //        ChangeState(State.CHASE);
    //    }
    //}

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
