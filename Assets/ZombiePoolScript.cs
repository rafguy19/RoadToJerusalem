using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePoolScript : MonoBehaviour
{
    private GameObject player;
    private GameObject spitter;
    private PlayerHealth playerHealth;
    private float timer = 5;
    private BasicZombieAttack basicZombieAttack;
    private float tickInterval = 1f;
    private bool isDOTActive = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        spitter = GameObject.FindGameObjectWithTag("Spitter");
        basicZombieAttack = spitter.GetComponentInChildren<BasicZombieAttack>();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            if (!isDOTActive)
            {
                StartCoroutine(DOT());
            }
        }
    }

    IEnumerator DOT()
    {
        isDOTActive = true;
        float elapsedTime = 0f;

        while (elapsedTime < timer)
        {
            playerHealth.TakeDamage(basicZombieAttack.enemyattackDmg);
            elapsedTime += Time.deltaTime;
            yield return new WaitForSeconds(tickInterval);
        }
        isDOTActive = false;
    }
}
