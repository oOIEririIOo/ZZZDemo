using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllEnemyController : SingleMonoBase<AllEnemyController>
{

    public List<EnemyController> enemies;
    public Stack<EnemyController> parryStack;
    public List<EnemyController> parryList;
    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        enemies = new List<EnemyController>();
        parryStack = new Stack<EnemyController>();
    }

    public void AddParryList(EnemyController enemy)
    {
        parryList.Add(enemy);
    }

    public void RemoveParryList(EnemyController enemy)
    {
        if(parryList.Contains(enemy))
        {
            parryList.Remove(enemy);
        }
        
    }

    public EnemyController PopParryList()
    {
        return parryList.Last();
    }

    public int CheckParryList()
    {
        return parryList.Count;
    }

    public void ClearParryList()
    {
        if(parryList.Count != 0)
        {
            parryList.Clear();
        }
        
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
