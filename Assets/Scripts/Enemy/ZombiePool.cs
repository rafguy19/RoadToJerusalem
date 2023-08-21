using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : MonoBehaviour
{
    public GameObject poisonPool;
    public Transform poisonPoolPos;

    private float timer;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 1.2)
        {
            timer += Time.deltaTime;

            if (timer > 2)
            {
                timer = 0;
                Pool();
            }
        }
    }
    private void Pool()
    {
        Instantiate(poisonPool, poisonPoolPos.position, Quaternion.identity);
    }
}
