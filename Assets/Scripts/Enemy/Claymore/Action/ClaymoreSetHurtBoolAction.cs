using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ClaymoreSetHurtBoolAction : FSMAction
{
    public override void OnStart()
    {
        base.OnStart();
        
    }

    public override TaskStatus OnUpdate()
    {
        enemyController.isHurt = false;
        return TaskStatus.Success;
    }
}
