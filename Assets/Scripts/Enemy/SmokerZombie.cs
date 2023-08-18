using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmokerZombie : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject smoke;

    private bool smokeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (smoke.activeSelf)
        {
            //smoke.transform.position = playerController.transform.position;
            Fog();
        }
    }

    private void Fog()
    {
        smoke.SetActive(true);
    }
}
