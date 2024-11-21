using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreWaitAction : FSMAction
{

    public float chaseDistance;
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        int redomIndex;
        redomIndex = Random.Range(0,2);
        if(redomIndex == 0)
        {
            enemyController.PlayAnimation("Walk_L");
        }
        else enemyController.PlayAnimation("Walk_R");
        enemyController.animator.Update(0f);
        
    }

    public override TaskStatus OnUpdate()
    {
        enemyController.LookToVector3(enemyController.player.position);
        if (enemyController.IsAnimationEnd() || enemyController.GetDistance() >= chaseDistance)
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Running;


    }

    public override void OnEnd()
    {
        base.OnEnd();
    }
}
