using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks; // список префабов дл€ разнообразной генерации
    public GameObject player; // игрок
    public float checkerRadius; // радиус дл€ создани€ чанков
    Vector3 noTerrainPosition; // место подход€щее дл€ создани€ чанка
    public LayerMask terrainMask;
    Player pl;
    public GameObject currentChunk;

    [Header("Optimization")]

    public List<GameObject> spawnedChunks;
    public GameObject latestChunk;
    public float maxOptimizationDistance;
    float optimizationDistance;
    public float optimizerCooldown;
    public float optimizerCooldownDuration;
    void Start()
    {
        pl = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    private void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        if(pl.move_dir.x == 1f && pl.move_dir.y == 0f) // вправо
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }

        else if (pl.move_dir.x == -1f && pl.move_dir.y == 0f) // влево
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }

        else if (pl.move_dir.x == 0f && pl.move_dir.y == 1f) // вверх
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Top").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Top").position;
                SpawnChunk();
            }
        }

        else if (pl.move_dir.x == 0f && pl.move_dir.y == -1f) // вниз
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Bot").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Bot").position;
                SpawnChunk();
            }
        }


        // диагональные направлени€ провер€ютс€ иначе, тк вектор нормализован

        else if (pl.move_dir.x > 0 && pl.move_dir.y > 0) // вправо вверх
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Top Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Top Right").position;
                SpawnChunk();
            }
        }

        else if (pl.move_dir.x > 0f && pl.move_dir.y < 0f) // вправо вниз
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Bot Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Bot Right").position;
                SpawnChunk();
            }
        }

        else if (pl.move_dir.x < 0f && pl.move_dir.y > 0f) // влево вверх
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Top Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Top Left").position;
                SpawnChunk();
            }
        }

        else if (pl.move_dir.x < 0f && pl.move_dir.y <0f) // влево вниз
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Bot Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Bot Left").position;
                SpawnChunk();
            }
        }

    }




    private void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }


    void ChunkOptimizer()
    {
        optimizerCooldown -= Time.deltaTime;
        if (optimizerCooldown <= 0)
        {
            optimizerCooldown = optimizerCooldownDuration;
        }
        else
        {
            return;
        }
        foreach (GameObject chunk in spawnedChunks)
        {
            optimizationDistance = Vector3.Distance(pl.transform.position, chunk.transform.position);
            if (optimizationDistance > maxOptimizationDistance)
            {
                chunk.SetActive(false);

            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}
