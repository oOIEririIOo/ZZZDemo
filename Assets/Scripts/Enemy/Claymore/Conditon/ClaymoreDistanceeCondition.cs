using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreDistanceeCondition : FSMCondition
{
    public float minDistance;
    public float maxDistance;
    
    public override void OnStart()
    {
        base.OnStart();
        //Debug.Log(Vector3.Distance(enemyController.transform.position, enemyController.player.position));
        //enemyController.player = PlayerController.INSTANCE.playerModel.transform;
        //enemyController.agent.destination = enemyController.player.position;
        //enemyController.agent.isStopped = true;
    }
    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(enemyController.transform.position, enemyController.player.position) <= maxDistance && Vector3.Distance(enemyController.transform.position, enemyController.player.position) >= minDistance)
        {
            return TaskStatus.Success;
        }
        else return TaskStatus.Failure;
    }
    
}
