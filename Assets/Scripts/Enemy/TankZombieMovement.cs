using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankZombieMovement : MonoBehaviour
{
    public enum State
    {
        PATROL,
        CHASE,
        ATTACK,
        GUARD,
        DEATH,
    }
    [SerializeField]
    public State currentState;
    [SerializeField]
    public List<GameObject> waypoints = new List<GameObject>();
    private float attackDist = 2.6f;
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private TankAttack tankAttack;
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
    private PlayerController playerController;
    public bool isBlocking = false;
    public bool isColliding = false;
    private int prevHealth;
    public Animator animator;
    public Transform target;
    private bool dying = false;

    private void Awake()
    {
        attackTimer = 1;
        tankAttack = GetComponentInChildren<TankAttack>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponentInParent<Rigidbody2D>();
        ChangeState(currentState);
    }

    // Start is called before the first frame update
    void Start()
    {
        prevHealth = tankAttack.enemyCurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == State.PATROL)
        {
            animator.SetBool("Attack", false);
            Patrol();
        }
        else if (currentState == State.CHASE)
        {
            animator.SetBool("Attack", false);
            Chase();
        }
        else if (currentState == State.ATTACK)
        {
            Attack();
        }
        else if (currentState == State.GUARD)
        {
            animator.SetBool("Attack", false);
            Guard();
        }
        else if (currentState == State.DEATH)
        {
            Die();
        }
    }

    private void ChangeState(State next)
    {

        if (next == State.PATROL)
        {
            GetComponent<TankZombieAI>().enabled = true;
        }
        else if (next == State.CHASE)
        {
            GetComponent<TankZombieAI>().enabled = true;
        }
        else if (next == State.ATTACK)
        {
            GetComponent<TankZombieAI>().enabled = false;
        }
        else if (next == State.GUARD)
        {
            GetComponent<TankZombieAI>().enabled = true;
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


        if (Vector3.Distance(transform.position, target.transform.position) <= 7.0f)
        {
            ChangeState(State.CHASE);
        }
        else if (prevHealth != tankAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }

        if (tankAttack.enemyCurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
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
        else if (prevHealth != tankAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }
        if (tankAttack.enemyCurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            // Initiate the attack
            animator.SetTrigger("Attack");
            tankAttack.DealDamage();
            isAttacking = true;
            attackTimerCountdown = attackTimer; // Start the cooldown timer
        }

        if (isAttacking)
        {
            attackTimerCountdown -= Time.deltaTime;
            if (attackTimerCountdown <= 0)
            {
                isAttacking = false; // Reset the attacking flag
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
        else if (prevHealth != tankAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }
        if (tankAttack.enemyCurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }
    }

    private bool isDamageReduced;
    private float damageReductionDurationTimer = 3.0f;
    public float damageReductionDuration;
    private float damageReductionDelay;
    private float damageReductionDelayTimer = 6.0f;
    private float damageMultiplier = 0.4f;

    private void Guard()
    {
        if(prevHealth != tankAttack.enemyCurrentHealth && damageReductionDuration > 0f)
        {
            animator.SetTrigger("GetHit");
            damageReductionDuration -= Time.deltaTime;
            isDamageReduced = true;
            if (isBlocking && isColliding)
            {
                int reducedDamage = (int)(playerController.atkDmg * damageMultiplier);
                tankAttack.ReceiveDamage(reducedDamage);
                isColliding = false;
            }
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > attackDist)
        {
            prevHealth = tankAttack.enemyCurrentHealth;
            ChangeState(State.CHASE);
        }
        
        if (Vector3.Distance(transform.position, target.transform.position) <= attackDist)
        {
            animator.SetBool("Attack", true);
            prevHealth = tankAttack.enemyCurrentHealth;
            attackTimerCountdown = attackTimer;
            ChangeState(State.ATTACK);
        }

        if (damageReductionDuration <= 0f)
        {
            isDamageReduced = false;
            isBlocking = false;
            damageReductionDelay -= Time.deltaTime;
            if(damageReductionDelay <= 0f)
            {
                prevHealth = tankAttack.enemyCurrentHealth;
                damageReductionDuration = damageReductionDurationTimer;
                damageReductionDelay = damageReductionDelayTimer;
            }
        }

        if (tankAttack.enemyCurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }
    }



    private void Die()
    {
        if (!dying)
        {
            dying = true;
            animator.SetTrigger("Death");
        }
        StartCoroutine(DeleteBody());
    }

    IEnumerator DeleteBody()
    {
        yield return new WaitForSeconds(6);
        Destroy(tankAttack.entireZombie);
    }
}
