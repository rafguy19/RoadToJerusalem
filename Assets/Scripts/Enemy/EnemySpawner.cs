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

    public float xSize;
    public float ySize;
    public int stage = 0;
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
        while (stage < 1) // Keep spawning enemies indefinitely
        {
            yield return new WaitForSeconds(interval);

            Vector3 spawnPosition = FindValidSpawnPosition();
            if (spawnPosition != Vector3.zero)
            {
                Instantiate(enemy, spawnPosition, Quaternion.identity);
            }
        }
    }

    private Vector3 FindValidSpawnPosition()
    {
        Vector3 spawnPosition;

        float maxAttempts = 100; // Set a reasonable maximum number of attempts
        int currentAttempt = 0;

        do
        {
            spawnPosition = new Vector3(Random.Range(gameObject.transform.position.x - xSize/2, gameObject.transform.position.x + xSize / 2), Random.Range(gameObject.transform.position.y - ySize / 2, gameObject.transform.position.y + ySize / 2), 0);

            // Cast a ray upwards to check if the spawn position is clear on the desired layer
            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero, 0f, mapLayer);
            Debug.Log(hit.collider);

            if (hit.collider == null)
            {
                return spawnPosition;
            }

            currentAttempt++;
        } while (currentAttempt < maxAttempts);

        return Vector3.zero; // No valid spawn position found
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // Set the Gizmo color

        // Draw the wireframe cube (rectangle in 2D) at the specified position and size
        Gizmos.DrawWireCube(gameObject.transform.position, new Vector3(xSize,ySize,0));
    }
}
