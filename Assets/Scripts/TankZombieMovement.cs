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
    private ZombieAttack zombieAttack;
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
    private float prevHealth;

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
        prevHealth = zombieAttack.enemyCurrentHealth;
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
        else if (zombieAttack.enemyCurrentHealth != prevHealth)
        {
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
        else if (zombieAttack.enemyCurrentHealth != prevHealth)
        {
            ChangeState(State.GUARD);
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
        else if (zombieAttack.enemyCurrentHealth != prevHealth)
        {
            ChangeState(State.GUARD);
        }
    }

    private bool isDamageReduced = false;
    private float damageReductionDuration = 3.0f;
    private float damageMultiplier = 0.4f;
    private void Guard()
    {
        if (!isDamageReduced)
        {
            prevHealth = zombieAttack.enemyCurrentHealth;
            isDamageReduced = true;
            StartCoroutine(ReduceDamage());
        }

        if(zombieAttack.enemyCurrentHealth != prevHealth)
        {
            int reducedDamage = (int)(10 * damageMultiplier);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                zombieAttack.ReceiveDamage(reducedDamage);
            }
        }
    }

    IEnumerator ReduceDamage()
    {
        yield return new WaitForSeconds(damageReductionDuration);
        isDamageReduced = false;
    }
}
