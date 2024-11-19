using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    public override void Enter()
    {
        base.Enter();
        enemyController.PlayAnimation("Attack");
        LookToTarget(enemyModel.player);
    }

    public override void Update()
    {
        base.Update();
        if(IsAnimationEnd())
        {
            enemyController.SwitchState(EnemyState.Chase);
        }
    }
}
