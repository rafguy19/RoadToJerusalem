using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ar;
    private SpriteRenderer sr;
    [SerializeField]
    private PlayerScriptableObject stats;
    private PlayerAttackController playerAttackController;

    //player stats
    private float moveSpeed;
    private float atkDmg;

    private Vector2 moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        stats.Reset();
        moveSpeed = stats.speed;
        rb = gameObject.GetComponent<Rigidbody2D>();
        ar = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        playerAttackController = gameObject.GetComponent<PlayerAttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        ApplyAnimation();
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    void Inputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetMouseButton(0)) // Attacking
        {
            playerAttackController.AttackSelector(1);
        }
    }

    void ApplyAnimation()
    {
        if (rb.velocity != new Vector2(0,0))
        {
            ar.SetBool("Run", true);
        }
        else
        {
            ar.SetBool("Run", false);
        }
    }
    void ApplyMovement()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
    void UpdateDirection()
    {
        var dirH = Input.GetAxis("Horizontal");

        if (dirH > 0f)
        {
            sr.flipX = false;
        }
        else if (dirH < 0f)
        {
            sr.flipX = true;
        }
    }

    //player modifier
    public void UpdateDamage(int newDmgValue)
    {
        atkDmg = newDmgValue;
    }

}
