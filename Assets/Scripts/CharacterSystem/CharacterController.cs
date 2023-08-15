using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1f;

    [SerializeField]
    private int Starting_Health = 100;
    private Animator animator;

    //health bar system
    public HealthBar healthBar;
    public HealthSystem healthSystem;
    //running particle system
    public ParticleSystem dust;

    //sounds
    [SerializeField]
    private AudioClip drawSound, swingSound;


    [SerializeField]
    private AudioSource audioSource;

    //attack system
    bool isAttacking;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemylayers;
    public int damage;

    private bool isDead;
    //damage UI
    [SerializeField]
    private Damage_UI damageUI;

    [SerializeField]
    private Animation deathFade;
    void Awake()
    {
        isAttacking= false;
        animator = GetComponent<Animator>();
        healthSystem = new HealthSystem(Starting_Health);
        healthBar.Setup(healthSystem);
        isDead = false;

    }
    //use update to get keyboard input in real time(regular update rate)
    void Update()
    {
        //start coroutine when space is pressed and the attacking animation is not playing
        if (Input.GetKeyDown(KeyCode.Space) && isAttacking == false && isDead != true)
        {
            StartCoroutine(AttackCoroutine());

        }
    }

    public void UpdateDamage(int newDamage)
    {
        damageUI.UpdateCurrentDamage(newDamage);
        damage = newDamage;
    }

    [SerializeField]
    private GameEvent gameEvent;
    //use fixed update for any physics related code to prevent calculations being affected by framerates 
    void FixedUpdate()
    {
        
        //player dies if health = 0
        if(healthSystem.GetHealth() == 0 && isDead == false)
        {
            StartCoroutine(DeathFadeIn());
        }

        //if player not attacking, he may move, he may also not move if the game has ended
        if (isAttacking == false && gameEvent.Won != true && isDead != true)
        {
            // Get the current position of the game object
            Vector2 currentPos = transform.position;
            // Get the input from horizontal and vertical axis - x and y
            Vector2 moveDirection = new Vector2(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
            //changes sprite Y scale according to direction
            if (moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-2, 2, 2);
            }
            else if (moveDirection.x > 0)
            {
                transform.localScale = new Vector3(2, 2, 2);
            }

            // To ensure the vector is unit length
            moveDirection = Vector2.ClampMagnitude(moveDirection, 1);


            // Calculate the new position based on velocity (moveDirection * movementSpeed)
            Vector2 movement = moveDirection * movementSpeed;

         
            //updates the animation
            if (movement == new Vector2(0, 0))
            {
                animator.SetFloat("animationValue", 0);
            }
            //play dust animation when moving
            else
            {
                CreateDust();
                animator.SetFloat("animationValue", 1);
            }

            //move player to new position
            transform.position = currentPos + movement * Time.fixedDeltaTime;
        }
    }
    void CreateDust()
    {
        dust.Play();
    }
    private IEnumerator DeathFadeIn()
    {
        animator.SetTrigger("isDead");
        isDead = true;
        yield return new WaitForSeconds(2);
        deathFade.Play();

        yield return new WaitForSeconds(0.5f);
        //load death scene
        SceneManager.LoadScene("Death Screen");
    }
    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;


        // Play attack animation
        animator.SetTrigger("Attack");

        // Wait for attack animation to finish
        AnimationClip[] Attackanimation = animator.runtimeAnimatorController.animationClips;
        float attackAnimationLength = 0f;

        foreach (var frame in Attackanimation)
        {
            if (frame.name == "Charge")
            {
                attackAnimationLength = frame.length;
                break;
            }
        }

        //waits until animation ends
        yield return new WaitForSeconds(attackAnimationLength);

        //attack only swing animation is playing
        Attack();

        //play swing sound
        audioSource.PlayOneShot(swingSound);

        foreach (var frame in Attackanimation)
        {
            if (frame.name == "Swing")
            {
                attackAnimationLength = frame.length;
                break;
            }
        }

        yield return new WaitForSeconds(attackAnimationLength);

        // Enable movement after attack animation
        isAttacking = false;
    }

    void Attack()
    {

        //detect enimies in range

        //an array to store all the enimies hit
        Collider2D[] hitEnimies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemylayers);

        //do dmg to enimies
        foreach (Collider2D enemy in hitEnimies)
        {
            enemy.GetComponent<Enemy>().takeDamage(damage);
        }
    }
    //draws a sphere to see attack point
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
