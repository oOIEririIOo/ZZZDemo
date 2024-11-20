using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreWaitAction : FSMAction
{
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Walk_L");
        enemyController.animator.Update(0f);
        enemyController.animator.speed = 1f;

    }

    public override TaskStatus OnUpdate()
    {
        enemyController.LookToVector3(enemyController.player.position);
        if (enemyController.IsAnimationEnd())
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
