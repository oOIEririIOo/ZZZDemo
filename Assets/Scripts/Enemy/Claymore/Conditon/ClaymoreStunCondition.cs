using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ClaymoreStunCondition : FSMCondition
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController.isStun)
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}
