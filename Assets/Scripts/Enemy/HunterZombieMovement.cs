using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
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
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
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
        collided = false;
        QTE.SetActive(false);
        attackTimer = 1;
        hunterZombieAttack = GetComponent<HunterZombieAttack>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hunterZombieAI = GetComponent<HunterZombieAI>();
        rb = GetComponent<Rigidbody2D>();
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
        //Debug.Log(attackTimerCountdown);
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
        Debug.Log("AFK");
        rb.velocity = Vector2.zero;
        idleTime += Time.deltaTime;

        if (idleTime >= 7)
        {
            ChangeState(State.PATROL);
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(waypoints[targetIndex].transform.position, transform.position) <= 1)
        {
            targetIndex++;
            targetIndex %= waypoints.Count;
            ChangeState(State.IDLE);
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

        if (Vector3.Distance(transform.position, target.transform.position) <= pounceDist)
        {
            attackTimerCountdown = attackTimer;
            playerCurrentPos = target.transform.position;
            direction = playerCurrentPos - transform.position;
            direction.Normalize();
            ChangeState(State.POUNCE);
        }
    }

    private void Pounce()
    {
        Debug.Log(playerCurrentPos);
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
            
            rb.velocity = Vector2.zero;
            // Check if the gameObject is colliding with objects tagged as "Player"
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(hunterZombieAttack.enemyattackPoint.position, hunterZombieAttack.enemyattackRange, hunterZombieAttack.playerLayers);
            foreach (Collider2D player in hitPlayer)
            {
                if (player.CompareTag("Player"))
                {
                    Debug.Log("HIT");
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
        Debug.Log("ATTACKING");

        playerController.Jumped = true;
        playerController.rb.velocity = Vector2.zero;
        QTE.SetActive(true);

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
