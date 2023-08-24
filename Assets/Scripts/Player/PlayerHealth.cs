using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth);
    }
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    public void Heal(int heal)
    {
        currentHealth += heal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }
}
