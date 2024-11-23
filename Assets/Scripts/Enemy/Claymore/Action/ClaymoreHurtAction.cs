using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
public class ClaymoreHurtAction : FSMAction
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController.isHurt)
        {
            return TaskStatus.Running;
        }

        else
        {
            return TaskStatus.Failure;
        }
    }
}
