using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterZombieAttack : BasicZombieAttack
{
    private HunterZombieMovement zombieMovement;

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponent<HunterZombieMovement>();
    }


    public void deleteZombie()
    {
        Destroy(entireZombie);
    }
    new public void DealDamage()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyattackPoint.position, enemyattackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            CinemachineShake.Instance.ShakeCamera(1, .1f);
            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);
        }
        zombieMovement.isAttacking= false;
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
