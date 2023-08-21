using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class SmokerZombie : MonoBehaviour
{
    public VisualEffect smoke;
    private BasicZombieAttack basicZombieAttack;
    public GameObject smoker;
    // Start is called before the first frame update
    void Start()
    {
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();
        smoke.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (smoker.activeInHierarchy)
        {
            Fog();
        }
        else
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            basicZombieAttack.ReceiveDamage(10);
        }
    }

    private void Fog()
    {
        //if (basicZombieAttack.enemyCurrentHealth > 0)
        //{
        //    smoke.enabled = true;
        //}
        //else if (basicZombieAttack.enemyCurrentHealth <= 0)
        //{
        //    smoke.enabled = false;
        //    Destroy(smoker);
        //}
    }
}
