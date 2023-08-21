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
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
    private int prevHealth;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 1;
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();
        prevHealth = basicZombieAttack.enemyCurrentHealth;
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
        else if (currentState == State.GUARD)
        {
            Guard();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            basicZombieAttack.ReceiveDamage(10);
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
        if (Vector3.Distance(waypoints[targetIndex].transform.position, transform.position) <= 0.5f)
        {
            targetIndex++;
            targetIndex %= waypoints.Count;
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
        else if (prevHealth != basicZombieAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }
    }

    private void Attack()
    {
        isAttacking = false;
        if (!isAttacking)
        {
            attackTimerCountdown -= Time.deltaTime;
            if (attackTimerCountdown <= 0)
            {
                basicZombieAttack.DealDamage();
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
        else if (prevHealth != basicZombieAttack.enemyCurrentHealth)
        {
            damageReductionDelay = damageReductionDelayTimer;
            damageReductionDuration = damageReductionDurationTimer;
            ChangeState(State.GUARD);
        }
    }

    private bool isDamageReduced;
    private float damageReductionDurationTimer = 3.0f;
    private float damageReductionDuration;
    private float damageReductionDelay;
    private float damageReductionDelayTimer = 6.0f;
    private float damageMultiplier = 0.4f;

    private void Guard()
    {
        if(prevHealth != basicZombieAttack.enemyCurrentHealth && damageReductionDuration > 0f)
        {
            damageReductionDuration -= Time.deltaTime;
            isDamageReduced = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int reducedDamage = (int)(10 * damageMultiplier);
                basicZombieAttack.ReceiveDamage(reducedDamage);
            }
        }
        if (damageReductionDuration <= 0f)
        {
            isDamageReduced = false;
            damageReductionDelay -= Time.deltaTime;
            if(damageReductionDelay <= 0f)
            {
                prevHealth = basicZombieAttack.enemyCurrentHealth;
                damageReductionDuration = damageReductionDurationTimer;
                damageReductionDelay = damageReductionDelayTimer;
            }
        }
    }
}
