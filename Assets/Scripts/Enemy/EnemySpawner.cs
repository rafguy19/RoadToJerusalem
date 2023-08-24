using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombieObject;
    [SerializeField]
    private GameObject hunterObject;
    [SerializeField]
    private GameObject tankObject;
    //[SerializeField]
    //private GameObject smokerObject;
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
    //[SerializeField]
    //private float smokerInterval = 10f;
    [SerializeField]
    private float boomerInterval = 7f;
    [SerializeField]
    private float spitterInterval = 8f;

    private float distance;
    private GameObject player;
    public Tilemap walkableTilemap;
    public float xSize;
    public float ySize;
    private bool withinArea = false;
    private bool coroutineUsed = false;
    public int stage = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        withinArea = PlayerInArea();
        if (withinArea && !coroutineUsed)
        {
            StartCoroutine(spawnEnemy(zombieInterval, zombieObject));
            StartCoroutine(spawnEnemy(hunterInterval, hunterObject));
            StartCoroutine(spawnEnemy(tankInterval, tankObject));
            //StartCoroutine(spawnEnemy(smokerInterval, smokerObject));
            StartCoroutine(spawnEnemy(boomerInterval, boomerObject));
            StartCoroutine(spawnEnemy(spitterInterval, spitterObject));
            coroutineUsed = true;
        }
        else if (!withinArea)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        Vector3 spawnPosition = FindValidSpawnPosition();
        if (spawnPosition != Vector3.zero)
        {
            Instantiate(enemy, spawnPosition, Quaternion.identity);
            coroutineUsed = false;
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

            if (IsPositionWalkable(spawnPosition))
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

    private bool PlayerInArea()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);

        return distance <= xSize / 2 && distance <= ySize / 2;
    }

    public bool IsPositionWalkable(Vector3 position)
    {
        // Convert the world position to a cell position in the tilemap
        Vector3Int cellPosition = walkableTilemap.WorldToCell(position);

        // Get the tile at the cell position
        TileBase tile = walkableTilemap.GetTile(cellPosition);

        // Check if the tile is not null (i.e., there's a tile there)
        if (tile != null)
        {
            // This position is walkable because there's a tile in the "walkable" layer.
            return true;
        }
        else
        {
            // This position is not walkable because there's no tile in the "walkable" layer.
            return false;
        }
    }

}
