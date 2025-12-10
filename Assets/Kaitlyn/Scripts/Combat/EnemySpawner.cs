using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemySpawnPoint> spawners;

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
        foreach (EnemySpawnPoint esp in spawners)
        {
            if (tm.isNight && esp.canSpawn)
            {
                StartCoroutine(SpawnEnemies(esp));
            }
        }

        #region dificulty scaling

        if (tm.Days == 0)
        {
            return;
        }
        if (tm.Days == 5 && tm.Hours == 7 && tm.Minutes == 0) //morning of day 5
        {
            foreach(EnemySpawnPoint esp in spawners)
            {
                ChangeRespawnTime(esp.ID);
            }
        }
        else if(tm.Days == 10 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (EnemySpawnPoint esp in spawners)
            {
                ChangeRespawnTime(esp.ID);
            }
        }
        else if(tm.Days == 15 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (EnemySpawnPoint esp in spawners)
            {
                ChangeRespawnTime(esp.ID);
            }
        }
        else if(tm.Days == 20 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (EnemySpawnPoint esp in spawners)
            {
                ChangeRespawnTime(esp.ID);
            }
        }
        else if(tm.Days == 30 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (EnemySpawnPoint esp in spawners)
            {
                ChangeRespawnTime(esp.ID);
            }
        }
        else if (tm.Days == 40 && tm.Hours == 7 && tm.Minutes == 0)
        {
            foreach (EnemySpawnPoint esp in spawners)
            {
                ChangeRespawnTime(esp.ID);
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

        Instantiate(esp.enemyType, esp.spawnPoint.position, Quaternion.identity);

        yield return new WaitForSeconds(esp.respawnTime);

        esp.canSpawn = true;
    }

    void ChangeRespawnTime(int enemyID)
    {
        foreach (EnemySpawnPoint esp in spawners)
        {
            if(esp.ID == enemyID)
            {
                float newCooldown = esp.respawnTime * 0.8f;
                esp.respawnTime = newCooldown;
                break;
            }
        }
    }
}

[System.Serializable]
public class EnemySpawnPoint
{
    [Tooltip("Start at 1 and go on from there. 1 will be the weakest enemy type with the lowest cooldown.")] public int ID;
    public GameObject enemyType;
    public Transform spawnPoint;
    public float respawnTime;
    public bool canSpawn;
}
