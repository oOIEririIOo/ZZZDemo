using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ClaymoreFindPlayerAction : FSMAction
{
    public override void OnStart()
    {
        base.OnStart();
        enemyController.player = PlayerController.INSTANCE.playerModel.transform;
    }
    public override TaskStatus OnUpdate()
    {
        return base.OnUpdate();
    }
}
