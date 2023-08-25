using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerMeleeController playerMeleeController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerMeleeController = GetComponentInParent<PlayerMeleeController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spitter"))
        {
            Debug.Log("HIT SOMETHING");
            collision.gameObject.GetComponent<BasicZombieAttack>().ReceiveDamage(playerController.atkDmg);

        }   

    }
}
