using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnInterval = 3f;
    public int maxEnemies = 5;
    public float spawnRadius = 2f; 

    private int currentEnemies = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemies >= maxEnemies) return;

        
        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0f,
            Random.Range(-spawnRadius, spawnRadius)
        );

        Instantiate(enemy, transform.position + randomOffset, Quaternion.identity);
        currentEnemies++;
    }

    public void EnemyDied()
    {
        currentEnemies--;
    }
}
