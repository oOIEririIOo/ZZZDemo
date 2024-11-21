using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreWalkAction : FSMAction
{

    public float maxDistance;
    public float minDistance;
    public float distance;
    public float chaseDistance;
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        distance = Random.Range(minDistance, maxDistance);
        if(enemyController.GetDistance() <= distance)
        {
            enemyController.PlayAnimation("Walk_B_Start");
        }
        else
        {
            enemyController.PlayAnimation("Walk_F_Start");
        }
        
        enemyController.animator.Update(0f);
        enemyController.agent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        enemyController.agent.destination = enemyController.player.position;

        if ((enemyController.GetDistance() <= distance+0.2f && enemyController.GetDistance() >= distance-0.2f) || enemyController.GetDistance() >= chaseDistance)
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
