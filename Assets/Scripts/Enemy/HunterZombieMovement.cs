using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HunterZombieMovement : MonoBehaviour
{
    public enum State
    {
        IDLE,
        PATROL,
        CHASE,
        POUNCE,
        ATTACK,
    }

    [SerializeField]
    public State currentState;
    [SerializeField]
    public List<GameObject> waypoints = new List<GameObject>();
    private HunterZombieAI hunterZombieAI;
    private PlayerController playerController;
    [SerializeField]
    private QT_Event qt_Event;
    public GameObject QTE;

    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private HunterZombieAttack hunterZombieAttack;
    public bool isAttacking = false;
    
    private float idleTime;

    public Transform target;

    public Transform detectionRadius;
    public float detectionRange;

    //for pouncing
    public Transform pounceRadius;
    public float pounceDist;
    bool collided;
    Vector3 playerCurrentPos;
    Vector3 direction;
    private bool jumped;

    //for chasing
    private bool spotted;
    private bool growled;
    float spottedDelay=3;

    Animator animator;

    //audio
    private AudioSource audioSource;
    public AudioClip hunterHit;
    public AudioClip hunterJump;
    public AudioClip hunterSpot;
    public AudioClip hunterWalk;
    private void OnDrawGizmosSelected()
    {
        if (pounceRadius == null)
        {
            return;
        }
        if (detectionRadius == null)
        {
            return;
        }
        
        Gizmos.DrawWireSphere(pounceRadius.position, pounceDist);
        Gizmos.DrawWireSphere(detectionRadius.position, detectionRange);
    }
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        jumped = false;
        spotted = false;
        growled = false;
        animator = GetComponent<Animator>();
        collided = false;
        QTE.SetActive(false);
        hunterZombieAttack = GetComponent<HunterZombieAttack>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hunterZombieAI = GetComponent<HunterZombieAI>();
        rb = GetComponent<Rigidbody2D>();
        ChangeState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        if(hunterZombieAttack.isDead==false)
        {
            if (currentState == State.IDLE)
            {
                animator.SetBool("isWalking", false);
                Idle();
            }
            else if (currentState == State.PATROL)
            {
                animator.SetBool("isWalking", true);
                Patrol();
            }
            else if (currentState == State.CHASE)
            {
                animator.SetBool("isWalking", true);
                qt_Event.GetComponent<Image>().fillAmount = qt_Event.fillAmount;
                qt_Event.fillAmount = 0;
                Chase();
            }
            else if (currentState == State.POUNCE)
            {

                Pounce();
            }
            else if (currentState == State.ATTACK)
            {

                Attack();
            }
        }
       else
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
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
            GetComponent<HunterZombieAI>().enabled = true;
        }
        currentState = next;
    }

    private void Idle()
    {
        collided = false;
        rb.velocity = Vector2.zero;
        idleTime += Time.deltaTime;

        if (idleTime >= 7)
        {
            if(Vector3.Distance(transform.position, target.transform.position) <= detectionRange)
            {
                ChangeState(State.CHASE);
            }
            else
            {
                audioSource.PlayOneShot(hunterWalk);
                ChangeState(State.PATROL);
            }

        }
    }

    private void Patrol()
    {
        if (waypoints.Count != 0)
        {
            if (Vector3.Distance(waypoints[targetIndex].transform.position, transform.position) <= 1)
            {
                targetIndex++;
                targetIndex %= waypoints.Count;
            }
        }


        if (Vector3.Distance(transform.position, target.transform.position) <= detectionRange)
        {
            rb.velocity = Vector2.zero;
            ChangeState(State.CHASE);
        }
    }

    private void Chase()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > detectionRange)
        {
           
            ChangeState(State.PATROL);
        }
        // to do hunter spotted growl
        if(Vector3.Distance(transform.position, target.transform.position)<=detectionRange&& Vector3.Distance(transform.position, target.transform.position)>pounceDist)
        {
            // to make it so that only one audio clip is played for spotted growl
            spotted = true;
            if (spotted == true && growled == false)
            {
                audioSource.PlayOneShot(hunterSpot);
                spotted = false;
                growled = true;
                spottedDelay = 4;
            }
            else if(growled == true)
            {
                spottedDelay -= Time.deltaTime;
               if(spottedDelay<=0)
               {
                    spotted = true;
                    growled = false;
               }
                
            }
           
        }
        if (Vector3.Distance(transform.position, target.transform.position) <= pounceDist)
        {
            
            playerCurrentPos = target.transform.position;
            direction = playerCurrentPos - transform.position;
            direction.Normalize();
            //to stop the spotted growl
            audioSource.Stop();
            if(direction.x < 0)
            {
                gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if(direction.x > 0)
            {
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            ChangeState(State.POUNCE);
        }
    }
    
    private void Pounce()
    {
        animator.SetBool("landed", false);
        if(jumped == false)
        {
            audioSource.PlayOneShot(hunterJump);

            animator.SetTrigger("pounce");
            jumped = true;
        }

        float distance = Vector3.Distance(transform.position, playerCurrentPos);

        // Check if the object has not reached the target.
        if (distance > 1) // You can adjust this threshold as needed.
        {
            // Move the object towards the target using Translate.
            transform.Translate(direction * 5 * Time.deltaTime);
        }
        else
        {
            collided = true;
        }

        if(collided == true)
        {
            jumped = false;
            animator.SetBool("landed", true);
            rb.velocity = Vector2.zero;
            // Check if the gameObject is colliding with objects tagged as "Player"
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(hunterZombieAttack.enemyattackPoint.position, hunterZombieAttack.enemyattackRange, hunterZombieAttack.playerLayers);
            foreach (Collider2D player in hitPlayer)
            {
                if (player.CompareTag("Player"))
                {
                    CinemachineShake.Instance.ShakeCamera(10, 1);
                    audioSource.PlayOneShot(hunterHit);
                    ChangeState(State.ATTACK);
                    break; // Exit the loop once we find a player
                }
            }
            // If no players were found, set currentState to IDLE
            if (currentState != State.ATTACK)
            {
                ChangeState(State.IDLE);
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(currentState == State.POUNCE)
        {
            if(collision.gameObject.CompareTag("Enemy") == false)
            {
                collided = true;
            }

        }
    }
    private void Attack()
    {

        collided = false;
        rb.velocity = Vector2.zero;
        playerController.rb.velocity = Vector2.zero;
        playerController.Jumped = true;

        QTE.SetActive(true);
        if(isAttacking == false)
        {
            animator.SetTrigger("attack");
            isAttacking = true;
        }
        if (qt_Event.fillAmount >= 1)
        {
            QTE.SetActive(false);
            playerController.Jumped = false;
            ChangeState(State.IDLE);
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) > pounceDist)
        {
            ChangeState(State.CHASE);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > detectionRange)
        {
            ChangeState(State.PATROL);
        }
    }
}
