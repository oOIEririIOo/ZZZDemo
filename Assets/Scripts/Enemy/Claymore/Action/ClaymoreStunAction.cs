using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;

public class ClaymoreStunAction : FSMAction
{
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Stun_Start");
        enemyController.characterStats.RemoveStun();
        enemyController.animator.Update(0f);

    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController.isStun == false)
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
