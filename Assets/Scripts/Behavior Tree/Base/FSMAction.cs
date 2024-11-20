using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class FSMAction : Action
{
    protected Claymore enemyController;

    public override void OnAwake()
    {
        base.OnAwake();
        enemyController = GetComponent<Claymore>();
    }

    public override void OnEnd()
    {
        base.OnEnd();
        //enemyController.PlayAnimation("New State");
        //enemyController.animator.speed = 0f;
    }
}
