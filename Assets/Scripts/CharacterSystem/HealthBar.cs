using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
    }

    private void Update()
    {
        transform.Find("Health").localScale = new Vector3(healthSystem.GetHealthPercent(), 1); 
    }
}
