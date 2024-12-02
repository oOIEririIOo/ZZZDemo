using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ClaymoreParriedCondition : FSMCondition
{
    public override void OnStart()
    {
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController.beParring)
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
}
