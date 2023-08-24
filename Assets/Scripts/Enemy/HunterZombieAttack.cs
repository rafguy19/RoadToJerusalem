using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterZombieAttack : BasicZombieAttack
{
    private HunterZombieMovement zombieMovement;

    Animator animator;

    public bool isDead;

    float deathTimer;
    private void Start()
    {
        deathTimer = 3;
        isDead = false;
        animator = GetComponent<Animator>();
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponent<HunterZombieMovement>();
    }
    private void Update()
    {
        if(enemyCurrentHealth <=0 && isDead == false)
        {
            animator.SetTrigger("dead");
            isDead = true;
        }
        if(isDead == true)
        {
            deathTimer -= Time.deltaTime;
            if(deathTimer <=0)
            {
                deleteZombie();
            }
        }
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
