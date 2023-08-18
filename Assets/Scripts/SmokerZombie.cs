using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmokerZombie : MonoBehaviour
{
    public enum State
    {
        IDLE,
        FOG,
    }

    [SerializeField]
    public State currentState;
    [SerializeField]
    public List<GameObject> waypoints = new List<GameObject>();
    private HunterZombieAI HunterZombieAI;
    private PlayerController playerController;
    private float pounceDist = 4f;
    private SpriteRenderer sr;
    public int targetIndex;
    private Rigidbody2D rb;
    private ZombieAttack zombieAttack;
    private bool isAttacking = false;
    private float attackTimer;
    private float attackTimerCountdown;
    private float idleTime;
    private float fogTime;
    public GameObject smoke;

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
        else if (currentState == State.FOG)
        {
            Fog();
        }
    }

    private void ChangeState(State next)
    {
        if (next == State.IDLE)
        {
            idleTime = 0.0f;
            GetComponent<HunterZombieAI>().enabled = false;
        }
        else if (next == State.FOG)
        {
            GetComponent<HunterZombieAI>().enabled = false;
        }
        currentState = next;
    }

    private void Idle()
    {
        idleTime += Time.deltaTime;

        if (idleTime >= 15.0f)
        {
            ChangeState(State.FOG);
            smoke.SetActive(true);
            idleTime -= Time.deltaTime;
        }
    }

    private void Fog()
    {
        fogTime += Time.deltaTime;

        if (fogTime >= 10.0f)
        {
            smoke.SetActive(false);
            ChangeState(State.IDLE);
        }
    }
}
