using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class FSMCondition : Conditional
{
    protected Claymore enemyController;
    public override void OnAwake()
    {
        base.OnAwake();
        enemyController = GetComponent<Claymore>();
    }
}
