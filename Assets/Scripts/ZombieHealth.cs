using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int enemyMaxHealth = 20;
    public int enemyCurrentHealth;
    public HealthBar enemyHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        enemyHealthBar.SetMaxHealth(enemyMaxHealth);
    }

    private void Update()
    {
        enemyHealthBar.SetHealth(enemyCurrentHealth);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReceiveDamage(5);
        }
    }

    public void ReceiveDamage(int playerDamage)
    {
        enemyCurrentHealth -= playerDamage;
        enemyHealthBar.SetHealth(enemyCurrentHealth);
    }
}
