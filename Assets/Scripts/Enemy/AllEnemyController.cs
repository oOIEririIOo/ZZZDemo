using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyController : SingleMonoBase<AllEnemyController>
{

    public List<EnemyController> enemies;
    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        enemies = new List<EnemyController>();
    }

    public List<EnemyController> GetAllEnemy()
    {
        return enemies;
    }
    public void AddEnemyList(EnemyController enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemyList(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }
}
