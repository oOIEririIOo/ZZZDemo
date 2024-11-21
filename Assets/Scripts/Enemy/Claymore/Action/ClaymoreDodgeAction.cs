using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreDodgeAction : FSMAction
{
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Dodge");
        enemyController.animator.Update(0f);

    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController.NormalizedTime() < 0.4f)
        {
            enemyController.LookToVector3(enemyController.player.position,5f);
        }
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
