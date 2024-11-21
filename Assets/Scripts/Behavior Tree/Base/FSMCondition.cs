using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class FSMCondition : Conditional
{
    protected EnemyController enemyController;
    public override void OnAwake()
    {
        base.OnAwake();
        enemyController = GetComponent<EnemyController>();
    }
}
