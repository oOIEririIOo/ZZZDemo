using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ClaymoreDeadCondition : FSMCondition
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if(enemyController.isDead)
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}
