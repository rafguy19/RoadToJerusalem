    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerAttack : BasicZombieAttack
{
  


    public Transform explosionRadius;
    public float explosionRange;

    public float explosionForce = 1;
    private BoomerMovement zombieMovement;

    public AudioClip explosionSound;
    new AudioSource audioSource;
    new private Collider2D collider;
    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        collider = GetComponent<Collider2D>();
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponentInParent<BoomerMovement>();
    }

    private void Update()
    {
        if (enemyCurrentHealth <= 0 && isDead == false)
        {
            zombieMovement.Explode();
            isDead = true;
        }
    }

    public void ExplosionKnockBack()
    {
        collider.enabled = false;
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(explosionRadius.position, explosionRange, playerLayers);

        foreach (Collider2D player in hitPlayer)
        {

            player.GetComponent<PlayerHealth>().TakeDamage(enemyattackDmg);

            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();



            Vector2 knockback = new Vector2(direction.x, direction.y) * explosionForce;



            player.GetComponent<PlayerController>().knockBacked = true;
            player.GetComponent<Rigidbody2D>().AddForce(knockback, ForceMode2D.Impulse);

        }
        CinemachineShake.Instance.ShakeCamera(10, .5f);
        audioSource.PlayOneShot(explosionSound);
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
