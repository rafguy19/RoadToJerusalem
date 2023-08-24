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
    private Animator animator;

    [SerializeField]
    private EnemySpawner enemySpawner;

    private BasicZombieMovement zombieMovement;

    public ParticleSystem blood;
    private void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponentInParent<BasicZombieMovement>();
    }

    private void Update()
    {

    }

    public void ReceiveDamage(int playerDamage)
    {
        blood.Play();
        enemyCurrentHealth -= playerDamage;
        if (enemyCurrentHealth <= 0)
        {
            //animator.SetTrigger("dead");
            Destroy(entireZombie, 0.5f);
            enemySpawner.zombieCount--;
        }
    }

    public void DealDamage()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(enemyattackPoint.position, enemyattackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);
        }
        //zombieMovement.isAttacking = false;
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
