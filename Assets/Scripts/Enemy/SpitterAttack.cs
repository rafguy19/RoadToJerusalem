using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterAttack : MonoBehaviour
{
    public int enemyMaxHealth = 20;
    public int enemyCurrentHealth;
    public GameObject entireZombie;
    public int enemyattackDmg;

    private SpitterMovement zombieMovement;

    public ParticleSystem blood;
    private void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        zombieMovement = gameObject.GetComponentInParent<SpitterMovement>();
    }

    private void Update()
    {
        if (enemyCurrentHealth <= 0)
            Destroy(entireZombie);
    }

    public void ReceiveDamage(int playerDamage)
    {
        blood.Play();
        enemyCurrentHealth -= playerDamage;
    }
}
