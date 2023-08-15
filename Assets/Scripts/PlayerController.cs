using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
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
        playerAttackController = gameObject.GetComponent<PlayerAttackController>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
    }

    private void FixedUpdate()
    {
        applyMovement();
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
    void applyMovement()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }


    //player modifier
    public void UpdateDamage(int newDmgValue)
    {
        atkDmg = newDmgValue;
    }

}
