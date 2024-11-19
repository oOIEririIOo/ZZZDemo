using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClaymoreIdleAction : FSMAction
{
    public override void OnAwake()
    {
        base.OnAwake();
        
    }
    
    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Idle",0.2f);

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
        enemyController.PlayAnimation("New State");
    }
}
