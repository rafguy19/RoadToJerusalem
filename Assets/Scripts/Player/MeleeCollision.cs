using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollision : MonoBehaviour
{
    private PlayerController playerController;
<<<<<<< Updated upstream
    private PlayerMeleeController playerMeleeController;
=======
<<<<<<< Updated upstream
=======
    private PlayerMeleeController playerMeleeController;
    private TankZombieMovement tankMovement;
>>>>>>> Stashed changes
>>>>>>> Stashed changes

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
<<<<<<< Updated upstream
        playerMeleeController = GetComponentInParent<PlayerMeleeController>();
=======
<<<<<<< Updated upstream
=======
        playerMeleeController = GetComponentInParent<PlayerMeleeController>();
        tankMovement = GameObject.FindGameObjectWithTag("Tank").GetComponent<TankZombieMovement>();
>>>>>>> Stashed changes
>>>>>>> Stashed changes
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerMeleeController.isAttack == true)
        {
<<<<<<< Updated upstream
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spitter"))
            {
                collision.gameObject.GetComponent<BasicZombieAttack>().ReceiveDamage(playerController.atkDmg);

            }
=======
<<<<<<< Updated upstream
            collision.gameObject.GetComponent<BasicZombieAttack>().ReceiveDamage(playerController.atkDmg);

=======
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<BasicZombieAttack>().ReceiveDamage(playerController.atkDmg);
            }
            else if (collision.gameObject.CompareTag("Boomer"))
            {
                collision.gameObject.GetComponent<BoomerAttack>().ReceiveDamage(playerController.atkDmg);
            }
            else if (collision.gameObject.CompareTag("Hunter"))
            {
                collision.gameObject.GetComponent<HunterZombieAttack>().ReceiveDamage(playerController.atkDmg);
            }
            else if (collision.gameObject.CompareTag("Spitter"))
            {
                collision.gameObject.GetComponent<SpitterAttack>().ReceiveDamage(playerController.atkDmg);
            }
            else if (collision.gameObject.CompareTag("Tank"))
            {
                tankMovement.isColliding = true;
                if (tankMovement.damageReductionDuration > 0f)
                    tankMovement.isBlocking = true;
                collision.gameObject.GetComponent<TankAttack>().ReceiveDamage(playerController.atkDmg);      
            }
>>>>>>> Stashed changes
>>>>>>> Stashed changes
        }

    }
}
