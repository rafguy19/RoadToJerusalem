using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpitScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    private float timer;
    private PlayerHealth playerHealth;
    private BasicZombieAttack basicZombieAttack;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        basicZombieAttack = GetComponentInChildren<BasicZombieAttack>();

        Vector3 Direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(Direction.x, Direction.y).normalized * force;

        //uncomment whne there is sprite for spit
        //float rot = Mathf.Atan2(-Direction.y, -Direction.x) * Mathf.Rad2Deg;
        //used for the rotation of the sprite to make it face the player +/- 90 based on player pos
        //transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 4)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(basicZombieAttack.enemyattackDmg);
            Destroy(gameObject);
        }
    }
}
