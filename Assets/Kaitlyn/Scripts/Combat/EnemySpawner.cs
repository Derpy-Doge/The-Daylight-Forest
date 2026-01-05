using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<LevelSpawnpoints> spawners;

    private TimeManager tm;

    void Awake()
    {
        tm = FindFirstObjectByType<TimeManager>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        foreach(LevelSpawnpoints level in spawners)
        {
            foreach (EnemySpawnPoint esp in level.spawners)
            {
                if (tm.isNight && esp.canSpawn)
                {
                    StartCoroutine(SpawnEnemies(esp));
                }
            }
        }
        
        #region dificulty scaling

        if (tm.Days == 0)
        {
            return;
        }
        if (tm.Days == 5 && tm.Hours == 7 && tm.Minutes == 0) //morning of day 5
        {
            foreach (LevelSpawnpoints level in spawners)
            {
                foreach (EnemySpawnPoint esp in level.spawners)
                {
                    ChangeRespawnTime(esp.ID);
                }
            }
           
        }
        else if(tm.Days == 10 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (LevelSpawnpoints level in spawners)
            {
                foreach (EnemySpawnPoint esp in level.spawners)
                {
                    ChangeRespawnTime(esp.ID);
                }
            }
            
        }
        else if(tm.Days == 15 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (LevelSpawnpoints level in spawners)
            {
                foreach (EnemySpawnPoint esp in level.spawners)
                {
                    ChangeRespawnTime(esp.ID);
                }
            }
            
        }
        else if(tm.Days == 20 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (LevelSpawnpoints level in spawners)
            {
                foreach (EnemySpawnPoint esp in level.spawners)
                {
                    ChangeRespawnTime(esp.ID);
                }
            }
            
        }
        else if(tm.Days == 30 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (LevelSpawnpoints level in spawners)
            {
                foreach (EnemySpawnPoint esp in level.spawners)
                {
                    ChangeRespawnTime(esp.ID);
                }
            }
            
        }
        else if (tm.Days == 40 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (LevelSpawnpoints level in spawners)
            {
                foreach (EnemySpawnPoint esp in level.spawners)
                {
                    ChangeRespawnTime(esp.ID);
                }
            }
            
        }
        else
        {
            return;
        }
        #endregion

    }

    public IEnumerator SpawnEnemies(EnemySpawnPoint esp)
    {
        esp.canSpawn = false;

        for (int i = 0; i < esp.spawnPoints.Count; i++)
        {
            Instantiate(esp.enemyType, esp.spawnPoints[i].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(esp.respawnTime);

        esp.canSpawn = true;
    }

    void ChangeRespawnTime(int enemyID)
    {
        foreach (LevelSpawnpoints level in spawners)
        {
            foreach (EnemySpawnPoint esp in level.spawners)
            {
                if (esp.ID == enemyID)
                {
                    float newCooldown = esp.respawnTime * 0.8f;
                    esp.respawnTime = newCooldown;
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class EnemySpawnPoint
{
    [Tooltip("Start at 1 and go on from there. 1 will be the weakest enemy type with the lowest cooldown.")] public int ID;
    public GameObject enemyType;
    public List<Transform> spawnPoints;
    public float respawnTime;
    public bool canSpawn;
}

[System.Serializable]
public class LevelSpawnpoints
{
    public int levelID;
    public List<EnemySpawnPoint> spawners;
}