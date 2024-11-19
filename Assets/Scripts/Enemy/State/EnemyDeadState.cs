using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyDeadState : EnemyStateBase
{
    public override void Enter()
    {
        base.Enter();
        enemyController.PlayAnimation("Dead");
        if(enemyController.GetComponent<LootSpawner>())
        {
            enemyController.GetComponent<LootSpawner>().SpawnLoot();
        }
    }

    public override void Update()
    {
        base.Update();
        if(IsAnimationEnd())
        {
            enemyController.isDead = true;
            enemyController.gameObject.SetActive(false);
            enemyController.DestroyModel();
        }
    }
}
