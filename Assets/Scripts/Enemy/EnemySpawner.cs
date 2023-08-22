using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombieObject;
    [SerializeField]
    private GameObject hunterObject;
    [SerializeField]
    private GameObject tankObject;
    [SerializeField]
    private GameObject smokerObject;
    [SerializeField]
    private GameObject boomerObject;
    [SerializeField]
    private GameObject spitterObject;

    public LayerMask mapLayer;

    [SerializeField]
    private float zombieInterval = 3.5f;
    [SerializeField]
    private float hunterInterval = 6f;
    [SerializeField]
    private float tankInterval = 15f;
    [SerializeField]
    private float smokerInterval = 10f;
    [SerializeField]
    private float boomerInterval = 7f;
    [SerializeField]
    private float spitterInterval = 8f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(zombieInterval, zombieObject));
        StartCoroutine(spawnEnemy(hunterInterval, hunterObject));
        StartCoroutine(spawnEnemy(tankInterval, tankObject));
        StartCoroutine(spawnEnemy(smokerInterval, smokerObject));
        StartCoroutine(spawnEnemy(boomerInterval, boomerObject));
        StartCoroutine(spawnEnemy(spitterInterval, spitterObject));
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        Vector3 spawnPosition = new Vector3(Random.Range(-7.5f, 17), Random.Range(-3, 6), 0);
        if (!Physics2D.Raycast(spawnPosition, Vector2.zero, Mathf.Infinity, mapLayer))
        {
            GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
        else
        {
            GameObject newEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
        StartCoroutine(spawnEnemy(interval, enemy));
    }
}
