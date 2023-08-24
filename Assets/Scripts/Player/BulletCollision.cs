using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spitter"))
        {
            Debug.Log(playerController.atkDmg);
            collision.gameObject.GetComponent<BasicZombieAttack>().ReceiveDamage(playerController.atkDmg);
            Destroy(transform.parent.gameObject);
        }
    }
}
