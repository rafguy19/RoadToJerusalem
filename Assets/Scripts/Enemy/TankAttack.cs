using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : BasicZombieAttack
{
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private PlayerHealth player;
    private TankZombieMovement tankMovement;
    public float hitForce = 500.0f;
    public AudioClip TankHit;


    private void Awake()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        audioSource = GetComponentInParent<AudioSource>();
        enemyCurrentHealth = enemyMaxHealth;
        tankMovement = gameObject.GetComponentInParent<TankZombieMovement>();
    }

    private void Update()
    {
        
    }

    new public void DealDamage()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyattackPoint.position, enemyattackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            audioSource.PlayOneShot(TankHit);
            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);
            KnockBack();

        }
        tankMovement.isAttacking = false;
    }

    private void KnockBack()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.Normalize();
        Vector2 knockback = new Vector2(direction.x, direction.y) * hitForce;
        playerController.knockBacked = true;
        playerRb.AddForce(knockback, ForceMode2D.Impulse);
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
