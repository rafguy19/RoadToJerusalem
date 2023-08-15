
using UnityEngine;

public class HealthSystem
{
    private int health;
    private int healthMax;
    public HealthSystem(int Startinghealth)
    {
        this.healthMax = 100;
        health = Startinghealth;
    }

    public int GetHealth()
    {

        return health;
    }
    public float GetHealthPercent()
    {
        return (float)health / healthMax;
    }
    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        //prevents health from dropping below limit
        if (health < 0)
        {
            health = 0;
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        //prevents health from exceeding limit
        if(health > healthMax)
        {
            health = healthMax;
        }

    }
}
