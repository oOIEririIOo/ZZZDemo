using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : SingleMonoBase<EnemySpawnManager>
{

    public Batches[] batches;
    public int currentBatch = 0;

    [System.Serializable]

    public class EnemySpawnInfo
    {
        public GameObject enemy;
        public EnemyController enemyController;
        public Vector3 spawnPoint;
        
    }

    [System.Serializable]
    public class Batches
    {
        public List<EnemySpawnInfo> enemiesInEveryBatch;
    }

    private void Start()
    {
        Spawn(0);
    }


    public void Spawn(int index)
    {
        foreach(EnemySpawnInfo enemyInfo in batches[index].enemiesInEveryBatch)
        {
            var enemy = Instantiate(enemyInfo.enemy);
            enemy.transform.position = enemyInfo.spawnPoint;
        }
    }

    public void FindEnemyInfo(EnemyController enemyController)
    {
        foreach (EnemySpawnInfo enemyInfo in batches[currentBatch].enemiesInEveryBatch)
        {
            if(enemyInfo.enemyController == enemyController)
            {
                Debug.Log("11111");
            }
        }
    }
    public void RemoveEnemy(EnemyController enemyController)
    {
        //batches[currentBatch].enemiesInEveryBatch.Remove(enemyController);
    }

}
