using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    public override void Enter()
    {
        base.Enter();
        enemyController.PlayAnimation("Idle");
    }

    public override void Update()
    {
        base.Update();

        if(animationPlayTime > 2f)
        {
            enemyController.SwitchState(EnemyState.Patrol);
        }
    }
}
