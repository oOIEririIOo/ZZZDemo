using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnState : EnemyStateBase
{
    public override void Enter()
    {
        base.Enter();

        enemyController.PlayAnimation("Turn");
        Debug.Log("TURN");
    }

    public override void Update()
    {
        base.Update();

        if(stateInfo.normalizedTime >= 0.9f)
        {
            if(enemyModel.player != null)
            {
                enemyController.SwitchState(EnemyState.React);
            }
            else
            {
                enemyController.SwitchState(EnemyState.Patrol);
            }
        }
    }

    
}
