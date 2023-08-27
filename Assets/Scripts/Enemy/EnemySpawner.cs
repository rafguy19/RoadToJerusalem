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
    private GameObject boomerObject;
    [SerializeField]
    private GameObject spitterObject;

    public LayerMask mapLayer;

    private float zombieInterval;
    private float hunterInterval;
    private float boomerInterval;
    private float spitterInterval;

    private GameObject player;
    public Tilemap walkableTilemap;
    public float xSize;
    public float ySize;
    private bool withinArea = false;
    private bool specialSpawned;
    bool specialSpawning;

    bool normalSpawned;
    // Start is called before the first frame update
    void Start()
    {
        zombieInterval = 0;
        boomerInterval = Random.Range(5, 20);
        spitterInterval = Random.Range(5, 20);
        hunterInterval = Random.Range(5, 20);
        specialSpawned = false;
        specialSpawning = false;
        normalSpawned = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        withinArea = PlayerInArea();


        if (withinArea)
        {
            if (normalSpawned == false)
            {
                StartCoroutine(SpawnEnemy(zombieInterval, zombieObject));
            }

            if (specialSpawning == false)
            {
                StartCoroutine(SpawnSpecial(hunterInterval, hunterObject));
                StartCoroutine(SpawnSpecial(boomerInterval, boomerObject));
                StartCoroutine(SpawnSpecial(spitterInterval, spitterObject));
                specialSpawning = true;
            }
        }
        else if (!withinArea)
        {
            StopAllCoroutines();
        }
    }



    //private IEnumerator StartSpawnEnemies(float delay)
    //{
    //    yield return new WaitForSeconds(delay);

    //    StartCoroutine(SpawnEnemy(zombieInterval, zombieObject));
    //    StartCoroutine(SpawnEnemy(hunterInterval, hunterObject));
    //    StartCoroutine(SpawnEnemy(tankInterval, tankObject));
    //    StartCoroutine(SpawnEnemy(boomerInterval, boomerObject));
    //    StartCoroutine(SpawnEnemy(spitterInterval, spitterObject));
    //}

    private IEnumerator SpawnEnemy(float interval, GameObject enemyPrefab)
    {
        normalSpawned = true;
        yield return new WaitForSeconds(interval);

        Vector3 spawnPosition = FindValidSpawnPosition();
        if (spawnPosition != Vector3.zero)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            zombieInterval = Random.Range(1, 5);
            normalSpawned = false;

        }
    }

    private IEnumerator SpawnSpecial(float interval, GameObject enemyPrefab)
    {
        yield return new WaitForSeconds(interval);

        Vector3 spawnPosition = FindValidSpawnPosition();
        if (spawnPosition != Vector3.zero && specialSpawned == false)
        {

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            specialSpawned = true;
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
        Vector3 areaCenter = gameObject.transform.position;
        float distanceX = Mathf.Abs(player.transform.position.x - areaCenter.x);
        float distanceY = Mathf.Abs(player.transform.position.y - areaCenter.y);

        return distanceX <= xSize / 2 && distanceY <= ySize / 2;
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
