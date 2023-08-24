using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterZombieAttack : BasicZombieAttack
{





    private HunterZombieMovement zombieMovement;
    private void Start()
    {
        enemyMaxHealth = 20;
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponent<HunterZombieMovement>();
    }

    private void Update()
    {
        if (enemyCurrentHealth <= 0)
        {
            Destroy(entireZombie);
        }
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
