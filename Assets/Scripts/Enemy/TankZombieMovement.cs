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


    private AudioSource audioSource;
    public AudioClip tankGrunt1;
    public AudioClip tankGrunt2;

    bool gurnting;
    float gurntingDelay = 3;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        basicZombieAttack = GetComponentInChildren<TankAttack>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponentInParent<Rigidbody2D>();
        prevHealth = basicZombieAttack.enemyCurrentHealth;
        ChangeState(currentState);
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
        //gruning sound
        if (gurnting == true)
        {
            gurntingDelay -= Time.deltaTime;
            if (gurntingDelay <= 0)
            {
                int randomGrunt = Random.Range(1, 3);
                switch (randomGrunt)
                {
                    case 1:
                        audioSource.PlayOneShot(tankGrunt1);
                        break;
                    case 2:
                        audioSource.PlayOneShot(tankGrunt2);
                        break;
                }
                gurntingDelay = Random.Range(1, 4);
            }
        }

        if (isDamaged())
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
            gurnting = true;
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


        if (Vector3.Distance(transform.position, target.transform.position) <= 15.0f)
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

        if (Vector3.Distance(transform.position, target.transform.position) <= 3f)
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
            attackTimerCountdown = attackTimer;
            isAttacking = true;
        }

        if (Vector3.Distance(transform.position, target.transform.position) > 3 && isAttacking == false)
        {
            ChangeState(State.CHASE);
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
