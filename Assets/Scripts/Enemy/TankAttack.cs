using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : BasicZombieAttack
{
    private TankZombieMovement tankMovement;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);
        }
        tankMovement.isAttacking = false;
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
