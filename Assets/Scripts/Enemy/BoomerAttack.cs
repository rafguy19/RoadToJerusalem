    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerAttack : MonoBehaviour
{
    public int enemyMaxHealth = 20;
    public int enemyCurrentHealth;
    public GameObject entireZombie;


    public Transform explosionRadius;
    public float explosionRange;

    public LayerMask playerLayers;
    public int enemyattackDmg;
    public float explosionForce = 1;
    private BoomerMovement zombieMovement;
    private void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponentInParent<BoomerMovement>();
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


    public void ExplosionKnockBack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(explosionRadius.position, explosionRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);

            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();

            Debug.Log("Direction: " + direction);

            Vector2 knockback = new Vector2(direction.x, direction.y) * explosionForce;
            Debug.Log("Knockback: " + knockback);


            player.GetComponent<PlayerController>().knockBacked = true;
            player.GetComponent<Rigidbody2D>().AddForce(knockback, ForceMode2D.Impulse);

            Debug.Log("Player Velocity: " + player.GetComponent<Rigidbody2D>().velocity);


        }
    }
    private void OnDrawGizmosSelected()
    {
        if (explosionRadius == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(explosionRadius.position, explosionRange);
    }
}
