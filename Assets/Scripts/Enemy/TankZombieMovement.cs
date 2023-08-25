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
    private float attackDist = 5.5f;
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    private TankAttack basicZombieAttack;
    public bool isAttacking;
    private float attackTimer;
    private float attackTimerCountdown;
    private PlayerController playerController;
    public int prevHealth;
    public Animator animator;
    public Transform target;
    private float damageMultiplier = 0.4f;
    private bool dying = false;
    public float knockBack = 500.0f;

    private void Start()
    {
        attackTimer = 1;
        basicZombieAttack = GetComponentInChildren<TankAttack>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        rb = GetComponentInParent<Rigidbody2D>();
        prevHealth = basicZombieAttack.enemyCurrentHealth;
        ChangeState(currentState);
    }

    bool isDamaged()
    {
        if(prevHealth != basicZombieAttack.enemyCurrentHealth)
        {
            return true;
        }
        else
        {
            prevHealth = basicZombieAttack.enemyCurrentHealth;
            return false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(isDamaged())
        {
            currentState = State.GUARD;
        }

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

        if (basicZombieAttack.enemyCurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }

        Debug.Log(Vector3.Distance(transform.position, target.transform.position));
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

        if (Vector3.Distance(transform.position, target.transform.position) <= 2.6f)
        {
            attackTimerCountdown = 0;
            ChangeState(State.ATTACK);
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

            Vector2 knockbackDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
            
            if (playerRb != null)
            {
                playerRb.AddForce(knockbackDirection * knockBack, ForceMode2D.Impulse);
            }

            attackTimerCountdown = attackTimer;

            isAttacking = true; 
        }

        if (Vector3.Distance(transform.position, target.transform.position) > 2.6f && isAttacking == false)
        {
            ChangeState(State.CHASE);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > 7.0f && isAttacking == false)
        {
            ChangeState(State.PATROL);
        }

        if (basicZombieAttack.enemyCurrentHealth <= 0)
        {
            ChangeState(State.DEATH);
        }
    }

    private void Guard()
    {
        if (isDamaged())
        {
            StartCoroutine(GuardingDamage());
        }


        if (Vector3.Distance(transform.position, target.transform.position) > attackDist)
        {
            prevHealth = basicZombieAttack.enemyCurrentHealth;
            ChangeState(State.CHASE);
        }

        //else if (Vector3.Distance(transform.position, target.transform.position) <= attackDist)
        //{
        //    animator.SetBool("Attack", true);
        //    prevHealth = basicZombieAttack.enemyCurrentHealth;
        //    attackTimerCountdown = attackTimer;
        //    ChangeState(State.ATTACK);
        //}
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
        yield return new WaitForSeconds(1);
        Destroy(basicZombieAttack.entireZombie);
    }

    IEnumerator GuardingDamage()
    {
        if (prevHealth != basicZombieAttack.enemyCurrentHealth)
        {
            int reducedDamage = (int)(playerController.atkDmg * damageMultiplier);
            if (Input.GetMouseButtonDown(0))
            {
                basicZombieAttack.ReceiveDamage(reducedDamage);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
