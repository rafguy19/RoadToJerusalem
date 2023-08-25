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
    private BasicZombieAttack basicZombieAttack;
    private BasicZombieMovement ZombieMovement;
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
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ZombieMovement = GameObject.FindGameObjectWithTag("Enemy").GetComponentInParent<BasicZombieMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        prevHealth = basicZombieAttack.enemyCurrentHealth;
        ChangeState(currentState);

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
        else if (prevHealth != basicZombieAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }

        if (basicZombieAttack.enemyCurrentHealth <= 0)
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
            attackTimerCountdown = 0;
            ChangeState(State.ATTACK);
        }
        else if (prevHealth != basicZombieAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }
        if (basicZombieAttack.enemyCurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }
    }

    private void Attack()
    {
        attackTimerCountdown -= Time.deltaTime;
        if (attackTimerCountdown <= 0)
        {
            animator.SetTrigger("Attack");
            attackTimerCountdown = attackTimer;
            ZombieMovement.isAttacking = true;
        }

        if (Vector3.Distance(transform.position, target.transform.position) > attackDist && ZombieMovement.isAttacking == false)
        {
            ChangeState(State.CHASE);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > 7.0f && ZombieMovement.isAttacking == false)
        {
            ChangeState(State.PATROL);
        }
        else if (prevHealth != basicZombieAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }
        if (basicZombieAttack.enemyCurrentHealth <= 0)
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
        if(prevHealth != basicZombieAttack.enemyCurrentHealth && damageReductionDuration > 0f)
        {
            animator.SetTrigger("GetHit");
            damageReductionDuration -= Time.deltaTime;
            isDamageReduced = true;
            if (isBlocking && isColliding)
            {
                int reducedDamage = (int)(playerController.atkDmg * damageMultiplier);
                basicZombieAttack.ReceiveDamage(reducedDamage);
                isColliding = false;
            }
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > attackDist)
        {
            prevHealth = basicZombieAttack.enemyCurrentHealth;
            ChangeState(State.CHASE);
        }
        
        if (Vector3.Distance(transform.position, target.transform.position) <= attackDist)
        {
            animator.SetBool("Attack", true);
            prevHealth = basicZombieAttack.enemyCurrentHealth;
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
                prevHealth = basicZombieAttack.enemyCurrentHealth;
                damageReductionDuration = damageReductionDurationTimer;
                damageReductionDelay = damageReductionDelayTimer;
            }
        }

        if (basicZombieAttack.enemyCurrentHealth <= 0)
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
        Destroy(basicZombieAttack.entireZombie);
    }
}
