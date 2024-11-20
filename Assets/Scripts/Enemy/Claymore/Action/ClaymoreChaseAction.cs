using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreChaseAction : FSMAction
{
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Run_Start");
        enemyController.agent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        enemyController.agent.destination = enemyController.player.position;

        if (enemyController.agent.remainingDistance <= 3f)
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Running;

    }

    public override void OnEnd()
    {
        base.OnEnd();
        enemyController.agent.isStopped = true;
    }
}
