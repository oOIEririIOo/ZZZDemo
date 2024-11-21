using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreBornAction : FSMAction
{
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Born");
        enemyController.animator.Update(0f);

    }

    public override TaskStatus OnUpdate()
    {
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
