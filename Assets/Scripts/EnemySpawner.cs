using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombieObject;

    public LayerMask mapLayer;

    [SerializeField]
    private float zombieInterval = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy(zombieInterval, zombieObject));
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
