using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName; // им€ волны
        public List<EnemyGroup> enemyGroups; // список всех групп разных врагов
        public int waveQuota; // сколько нужно заспавнить врагов
        public float spawnInterval; // интервал между спавном
        public int spawnCount; // число уже заспавненных врагов
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; 
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveCount;



    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInterval;
    public float minRadius = 12.5f;  // ћинимальное рассто€ние
    public float maxRadius = 17f;   // ћаксимальное рассто€ние
    bool isWaveActive = false;
    Transform playerLocation;
    void Start()
    {
        playerLocation = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
        //SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        //ѕровер€ем, закончилась ли текуща€ волна и нужно ли вызывать следующую
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)
        {
            StartCoroutine(BeginNextWave());
        }



        spawnTimer += Time.deltaTime;
        if(spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }
    IEnumerator BeginNextWave()
    {
        isWaveActive = true;
        yield return new WaitForSeconds(waveInterval);


        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if(enemyGroup.spawnCount < enemyGroup.enemyCount)
                {

                    // 1. √енерируем случайный угол от 0 до 2 * Pi
                    float angle = Random.Range(0f, Mathf.PI * 2);

                    // 2. √енерируем случайную дистанцию в заданном диапазоне
                    float distance = Random.Range(minRadius, maxRadius);

                    // 3. ¬ычисл€ем смещение по X и Y с помощью синуса и косинуса
                    Vector2 spawnOffset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

                    // 4. ѕолучаем итоговую позицию (позици€ игрока + смещение)
                    Vector2 spawnPosition = (Vector2)playerLocation.position + spawnOffset;

                    // 5. —оздаем врага
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }


    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}
