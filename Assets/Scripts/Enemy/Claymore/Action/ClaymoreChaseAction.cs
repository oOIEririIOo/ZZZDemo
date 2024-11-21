using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreChaseAction : FSMAction
{

    public float maxDistance;
    public float minDistance;
    public float distance;
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        distance = Random.Range(minDistance, maxDistance);
        enemyController.PlayAnimation("Run_Start");
        enemyController.animator.Update(0f);
        enemyController.agent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        enemyController.agent.destination = enemyController.player.position;

        if (enemyController.GetDistance() <= distance + 0.2f && enemyController.GetDistance() >= distance - 0.2f)
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Running;

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnEnd()
    {
        base.OnEnd();
        enemyController.agent.isStopped = true;
    }
}
