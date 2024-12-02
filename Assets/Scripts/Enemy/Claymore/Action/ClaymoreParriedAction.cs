using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ClaymoreParriedAction : FSMAction
{
    public override void OnAwake()
    {
        base.OnAwake();

    }

    public override void OnStart()
    {
        base.OnStart();
        enemyController.PlayAnimation("Hit_H_Front",0f);
        enemyController.animator.Update(0f);
        //������Һ�������˵ķ���
        Vector3 direction = (enemyController.player.transform.position - transform.position).normalized;
        //���ģ���泯����
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
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
        enemyController.beParring = false;
    }
}
