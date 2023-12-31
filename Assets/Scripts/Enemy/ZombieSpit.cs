using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpit : MonoBehaviour
{
    public GameObject Spit;
    public Transform SpitPos;
    public GameObject poisonPool;
    public bool enterSpit = false;
    private float timer;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 6)
        {
            if (!enterSpit)
            {
                Shoot();
                enterSpit = true;
            }
            else
            {
                timer += Time.deltaTime;

                if (timer > 4)
                {
                    timer = 0;
                    Shoot();
                }
            }

        }
    }

    private void Shoot()
    {
        Instantiate(Spit, SpitPos.position, Quaternion.identity);
    }
}
