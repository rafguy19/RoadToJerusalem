using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    public int enemyMaxHealth = 20;
    public int enemyCurrentHealth;
    public GameObject entireZombie;

    public Transform enemyattackPoint;
    public float enemyattackRange;
    public LayerMask playerLayers;
    public int enemyattackDmg;

    private void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
    }

    private void Update()
    {
        if (enemyCurrentHealth <= 0)
        {
            Destroy(entireZombie);
        }
    }

    public void ReceiveDamage(int playerDamage)
    {
        enemyCurrentHealth -= playerDamage;
    }

    public void DealDamage()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyattackPoint.position, enemyattackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);
        }
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