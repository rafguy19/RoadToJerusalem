using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private PlayerController playerController;
    private ArrowWheelController arrowWheelController;
    private PlayerHealth playerHealth;
    [SerializeField]
    private int firedamage;
    private float tickInterval = 1.5f;
    private bool isDotActive = false;
    private float timer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        arrowWheelController = GameObject.FindGameObjectWithTag("ArrowWheel").GetComponent<ArrowWheelController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Spitter"))
        {
            Debug.Log(playerController.atkDmg);
            collision.gameObject.GetComponent<BasicZombieAttack>().ReceiveDamage(playerController.atkDmg);
            switch (arrowWheelController.selectedArrow)
            {
                case 2: // Fire arrow
                    StartCoroutine(DOT(collision));
                    break;
                case 3: // Holy arrow
                    playerHealth.Heal(15);
                    break;
            }
            Destroy(transform.parent.gameObject);
        }
    }

    IEnumerator DOT(Collider2D collision)
    {
        isDotActive = true; // Mark the coroutine as active
        float elapsedTime = 0f;

        if (elapsedTime < timer)
        {
            collision.gameObject.GetComponent<BasicZombieAttack>().ReceiveDamage(firedamage);
            elapsedTime += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
        // Damage over time finished, you can add any necessary cleanup here
        isDotActive = false; // Mark the coroutine as no longer active
    }

}
