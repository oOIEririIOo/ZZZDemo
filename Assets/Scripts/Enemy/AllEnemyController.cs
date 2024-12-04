using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllEnemyController : SingleMonoBase<AllEnemyController>
{

    public List<EnemyController> enemies;
    public List<EnemyController> parryList;
    public List<EnemyController> stunedList;
    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        enemies = new List<EnemyController>();
        parryList = new List<EnemyController>();
        stunedList = new List<EnemyController>();
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


    public void AddStunedList(EnemyController enemy)
    {
        parryList.Add(enemy);
    }





    public void RemoveStunedList(EnemyController enemy)
    {
        if (stunedList.Contains(enemy))
        {
            stunedList.Remove(enemy);
        }

    }

    public EnemyController GetStunedListFirst()
    {
        return stunedList.First();
    }

    public int CheckStunedList()
    {
        return stunedList.Count;
    }

    public void ClearStunedList()
    {
        if (stunedList.Count != 0)
        {
            stunedList.Clear();
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
