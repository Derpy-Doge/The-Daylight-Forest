using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Vector3 spawnAreaMin;
    public Vector3 spawnAreaMax;
    public Vector3 spawnPosition;
    private int minEnemies = 1;
    private int maxEnemies = 3;

    private TimeManager tm;

    void Awake()
    {
        tm = Object.FindFirstObjectByType<TimeManager>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(tm.Days == 0)
        {
            minEnemies = 1;
            maxEnemies = 3;
        }
        else if(tm.Days <=4)
        {
            minEnemies = 2;
            maxEnemies = 4;
        }
        else if(tm.Days <=9)
        {
            minEnemies = 3;
            maxEnemies = 7;
        }
        else if(tm.Days <= 15)
        {
            minEnemies = 6;
            maxEnemies = 10;
        }
        else if(tm.Days <= 20)
        {
            minEnemies = 9;
            maxEnemies = 15;
        }
        else if (tm.Days <=40)
        {
            minEnemies = 12;
            maxEnemies = 20;
        }
        else
        {
             minEnemies = 15;
             maxEnemies = 30;
        }

        if (tm.isNight)
        {
            SpawnEnemiesOnLoad();
        }
        else if (!tm.isNight)
        {
            // No enemy spawning during the day
        }
    }

    void SpawnEnemiesOnLoad()
    {
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

        for(int i = 0; i < enemyCount; i++)
        {
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            if(spawnPoints.Length > 0)
            {
               spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            }
            else
            {
                float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
                float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
                float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);
            }

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
