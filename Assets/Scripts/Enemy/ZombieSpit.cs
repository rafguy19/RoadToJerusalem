using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpit : MonoBehaviour
{
    public GameObject Spit;
    public Transform SpitPos;

    private float timer;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);

        if (distance < 6)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        Instantiate(Spit, SpitPos.position, Quaternion.identity);
    }
}
