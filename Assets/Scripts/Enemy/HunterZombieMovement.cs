using System.Collections;
using System.Collections.Generic;
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
    }

    [SerializeField]
    public State currentState;
    [SerializeField]
    public List<GameObject> waypoints = new List<GameObject>();
    private HunterZombieAI HunterZombieAI;
    private PlayerController playerController;
    [SerializeField]
    private QT_Event qt_Event;
    public GameObject QTE;
    private float pounceDist = 4f;
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private ZombieAttack zombieAttack;
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
    private float idleTime;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        QTE.SetActive(false);
        attackTimer = 1;
        zombieAttack = GetComponentInChildren<ZombieAttack>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        HunterZombieAI = GetComponent<HunterZombieAI>();
        rb = GetComponentInParent<Rigidbody2D>();
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
            HunterZombieAI.EnemySpeed = 150;
            Patrol();
        }
        else if (currentState == State.CHASE)
        {
            qt_Event.GetComponent<Image>().fillAmount = qt_Event.fillAmount;
            qt_Event.fillAmount = 0;
            HunterZombieAI.EnemySpeed = 150;
            Chase();
        }
        else if (currentState == State.POUNCE)
        {
            Pounce();
        }
        Debug.Log(attackTimerCountdown);
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
        idleTime += Time.deltaTime;

        if (idleTime >= 1.5f)
        {
            ChangeState(State.PATROL);
        }
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
    }

    private void Chase()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > 7.0f)
        {

            ChangeState(State.PATROL);
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= pounceDist)
        {
            HunterZombieAI.EnemySpeed = 600;
            attackTimerCountdown = attackTimer;
            ChangeState(State.POUNCE);
        }
    }

    private void Pounce()
    {
        attackTimerCountdown -= Time.deltaTime;
        if (attackTimerCountdown <= 0)
        {
            playerController.Jumped = true;
            playerController.rb.velocity = Vector2.zero;
            zombieAttack.DealDamage();
            QTE.SetActive(true);
            attackTimerCountdown = attackTimer;
        }

        if (qt_Event.fillAmount >= 1)
        {
            QTE.SetActive(false);
            playerController.Jumped = false;
            attackTimerCountdown = attackTimer;
            ChangeState(State.IDLE);
            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) > pounceDist)
        {
            ChangeState(State.CHASE);
        }
        else if (Vector3.Distance(transform.position, target.transform.position) > 7.0f)
        {
            ChangeState(State.PATROL);
        }
    }
}
