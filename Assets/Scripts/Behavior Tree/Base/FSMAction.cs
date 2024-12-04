using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class FSMAction : Action
{
    protected EnemyController enemyController;

    public override void OnAwake()
    {
        base.OnAwake();
        enemyController = GetComponent<EnemyController>();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public override void OnEnd()
    {
        base.OnEnd();
        //enemyController.PlayAnimation("New State");
        //enemyController.animator.speed = 0f;
        for (int i = 0; i < enemyController.weapons.Length; i++)
        {
            enemyController.weapons[i].StopHit();
        }
        
    }
}
