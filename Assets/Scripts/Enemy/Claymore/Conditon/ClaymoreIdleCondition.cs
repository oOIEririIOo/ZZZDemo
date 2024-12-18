using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreIdleCondition : FSMCondition
{
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override TaskStatus OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            return  TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
