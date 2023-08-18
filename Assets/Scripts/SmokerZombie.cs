using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmokerZombie : MonoBehaviour
{
    public enum State
    {
        FOG,
        IDLE,
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
        if (currentState == State.FOG)
        {
            Fog();

        }
        else if (currentState == State.IDLE)
        {
            Idle();
        }
    }

    private void ChangeState(State next)
    {
        if (next == State.FOG)
        {
            fogTime = 0.0f;
            //GetComponent<HunterZombieAI>().enabled = false;
        }
        else if (next == State.IDLE)
        {
            idleTime = 0.0f;
            //GetComponent<HunterZombieAI>().enabled = false;
        }
        currentState = next;
    }

    private void Idle()
    {
        idleTime += Time.deltaTime;

        if (idleTime >= 15.0f)
        {
            smoke.SetActive(true);
            ChangeState(State.FOG);
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
