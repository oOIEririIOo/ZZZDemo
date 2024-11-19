using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyController : SingleMonoBase<AllEnemyController>
{

    public GameObject enemies;
    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void FindEnemy()
    {
        enemies = GameObject.FindGameObjectWithTag("Enemies");
    }
}
