using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReactState : EnemyStateBase
{
    public override void Enter()
    {
        base.Enter();
        enemyController.PlayAnimation("React");
    }

    public override void Update()
    {
        base.Update();
        LookToTarget(enemyModel.player);
        if (IsAnimationEnd())
        {
            enemyController.SwitchState(EnemyState.Chase);
        }
    }
}
