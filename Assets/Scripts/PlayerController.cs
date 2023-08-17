using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ar;
    private SpriteRenderer sr;
    [SerializeField]
    private PlayerScriptableObject stats;
    private PlayerAttackController playerAttackController;
    public VisualEffect vfxRenderer;

    //player stats
    private float moveSpeed;
    private float atkDmg;

    private Vector2 moveDirection;

    int type;

    //checking of weapon type
    private WeaponSystem playerCurrentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        stats.Reset();
        moveSpeed = stats.speed;
        rb = gameObject.GetComponent<Rigidbody2D>();
        ar = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        playerAttackController = gameObject.GetComponent<PlayerAttackController>();
        playerCurrentWeapon = gameObject.GetComponent<WeaponSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        ApplyAnimation();
        UpdateDirection();
        UpdateFogClear();

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
            //if nothing is equiped (fist)
            if(playerCurrentWeapon.getWeapon() == null)
            {
                playerAttackController.AttackSelector(2);
            }
            else
            {
                switch (playerCurrentWeapon.getWeapon().weaponType)
                {
                    case EquipableItem.WeaponType.BOW:
                        playerAttackController.AttackSelector(1);
                        break;
                    case EquipableItem.WeaponType.MELEE:
                        playerAttackController.AttackSelector(2);
                        break;
                    case EquipableItem.WeaponType.CROSSBOW:
                        playerAttackController.AttackSelector(3);
                        break;
                }
            }

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
    void UpdateFogClear()
    {
        if(vfxRenderer != null)
        {
            vfxRenderer.SetVector3("PlayerPosition", transform.localPosition);
        }
   
    }
    
}
