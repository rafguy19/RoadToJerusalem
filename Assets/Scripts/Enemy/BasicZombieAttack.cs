using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombieAttack : MonoBehaviour
{
    public int enemyMaxHealth = 20;
    public int enemyCurrentHealth;
    public GameObject entireZombie;

    public Transform enemyattackPoint;
    public float enemyattackRange;

    public LayerMask playerLayers;
    public int enemyattackDmg;

    private BasicZombieMovement zombieMovement;

    public ParticleSystem blood;

    private Animator animator;

    public bool isDead = false;

    protected AudioSource audioSource;
    public AudioClip bloodHitSound;
    protected float deathTimer = 3;
    private Collider2D zombieCollider;
    private void Start()
    {
        zombieCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponentInParent<BasicZombieMovement>();
    }

    private void Update()
    {

        if (enemyCurrentHealth <= 0 && isDead == false)
        {
            zombieCollider.enabled= false;
            animator.SetTrigger("dead");
            isDead = true;
        }
        if (isDead == true)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                deleteZombie();
            }
        }
    }
    public void deleteZombie()
    {
        Destroy(entireZombie);

        if (enemyCurrentHealth <= 0)
            Destroy(entireZombie);

    }
    public void ReceiveDamage(int playerDamage)
    {
        blood.Play();
        audioSource.PlayOneShot(bloodHitSound);
        enemyCurrentHealth -= playerDamage;
    }

    public void DealDamage()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyattackPoint.position, enemyattackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);
        }
        zombieMovement.isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyattackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(enemyattackPoint.position, enemyattackRange);
    }
}