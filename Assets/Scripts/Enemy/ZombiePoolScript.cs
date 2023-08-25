using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePoolScript : MonoBehaviour
{
    private GameObject player;
    private GameObject spitter;
    private Rigidbody2D rb;
    private float timer = 5f;
    private PlayerHealth playerHealth;
    private BasicZombieAttack basicZombieAttack;
    private ZombieSpit zombieSpit;
    private float tickInterval = 1.5f;
    private bool isDotActive = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        spitter = GameObject.FindGameObjectWithTag("Spitter");
        basicZombieAttack = spitter.GetComponentInChildren<BasicZombieAttack>();
        zombieSpit = spitter.GetComponent<ZombieSpit>();
        Destroy(gameObject, 5);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDotActive)
        {
            StartCoroutine(DOT());
        }
    }
    IEnumerator DOT()
    {
        isDotActive = true; // Mark the coroutine as active
        
        float elapsedTime = 0f;

        if (elapsedTime < timer)
        {
            playerHealth.TakeDamage(basicZombieAttack.enemyattackDmg);
            elapsedTime += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
        // Damage over time finished, you can add any necessary cleanup here
        isDotActive = false; // Mark the coroutine as no longer active
    }
}
