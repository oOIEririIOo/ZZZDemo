using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyChaseState : EnemyStateBase
{

    public override void Enter()
    {
        base.Enter();
        enemyController.PlayAnimation("Run");
    }
    public override void Update()
    {
        base.Update();
        enemyModel.agent.nextPosition = new Vector3(enemyModel.animator.rootPosition.x, enemyModel.animator.rootPosition.y, enemyModel.animator.rootPosition.z);
        if (enemyModel.player != null)
        {
            LookToTarget(enemyModel.player);
            enemyModel.agent.destination = enemyModel.player.position;
            if (enemyModel.agent.remainingDistance <= 0.7f)
            {
                enemyController.SwitchState(EnemyState.Attack);
            }
        }
        else
        { 
            enemyController.SwitchState(EnemyState.Idle);
        }
    }
}
