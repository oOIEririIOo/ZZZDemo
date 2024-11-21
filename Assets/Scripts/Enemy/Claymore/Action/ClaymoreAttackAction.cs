using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ClaymoreAttackAction : FSMAction
{
    public int attackIndex;
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Attack_"+ attackIndex, 0.2f);
        enemyController.animator.Update(0f);
        enemyController.isAttacking = true;
    }

    public override TaskStatus OnUpdate()
    {
        /*
        if(enemyController.NormalizedTime()<0.4f)
        {
            enemyController.LookToVector3(enemyController.player.position,3.5f);
        }
        */
        if (enemyController.IsAnimationEnd())
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Running;


    }

    public override void OnEnd()
    {
        base.OnEnd();
        enemyController.isAttacking = false;
    }
}
