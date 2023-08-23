     using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    [SerializeField]
    private PlayerScriptableObject stats;
    private PlayerAttackController playerAttackController;
    private HunterZombieMovement hunterZombieMovement;
    public bool Jumped;
    [SerializeField]
    private GameObject attackpoint;
    public VisualEffect vfxRenderer;
    [SerializeField]
    public GameObject qt_Event;
    [SerializeField]
    GameObject slider;

    //player stats
    private float moveSpeed;
    public int atkDmg;

    private Vector2 moveDirection;

    int type;

    public bool knockBacked;
    private float knockBackStunTimer;
    public float knockBackStunDuration = 0.25f;


    //checking of weapon type
    private WeaponSystem playerCurrentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        knockBackStunTimer = knockBackStunDuration;
        knockBacked =false;
        stats.Reset();
        moveSpeed = stats.speed;
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
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

        if (playerCurrentWeapon.getWeapon() != null)
        {
            if (playerCurrentWeapon.getWeapon().weaponType != EquipableItem.WeaponType.MELEE)
            {
                attackpoint.transform.localPosition = new Vector3(0.2f, 0, 0);
                slider.SetActive(true);
            }
            else
            {
                slider.SetActive(false);
            }
        }
        else
        {
            slider.SetActive(false);
        }
        if(knockBacked == true)
        {
            knockBackStunTimer -= Time.deltaTime;
            if(knockBackStunTimer <= 0)
            {
                knockBacked= false;
            }
        }
        else
        {
            knockBackStunTimer = knockBackStunDuration;
        }
    }


    private void FixedUpdate()
    {
        if(knockBacked == false || Jumped == false)
        {
            ApplyMovement();
        }

    }

    void Inputs()
    {
        if (!Jumped)
        {
            qt_Event.SetActive(false);

            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector2(moveX, moveY).normalized;

            if (Input.GetMouseButton(0)) // Attacking
            {
                //if nothing is equiped (fist)
                if (playerCurrentWeapon.getWeapon() == null)
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
        else
        {
            qt_Event.SetActive(true);
            moveDirection = Vector2.zero;
            rb.velocity.Set(0, 0);
        }
    }
    void ApplyAnimation()
    {
        if (rb.velocity != new Vector2(0, 0))
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }


    //player modifier
    public void UpdateDamage(int newDmgValue)
    {
        atkDmg = newDmgValue;
    }
    void UpdateFogClear()
    {
        if (vfxRenderer != null)
        {
            vfxRenderer.SetVector3("PlayerPosition", transform.localPosition);
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

}
