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
}
