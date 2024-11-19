using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : EnemyStateBase
{

    private Vector3 wayPoint;
    public override void Enter()
    {
        base.Enter();

        enemyController.PlayAnimation("Walk");
        GetNewWayPoint();
        MoveToPatrolPoints();
        
    }
    public override void Update()
    {
        base.Update();
        enemyModel.agent.nextPosition = new Vector3(enemyModel.animator.rootPosition.x, enemyModel.animator.rootPosition.y, enemyModel.animator.rootPosition.z);
        if(enemyModel.player != null)
        {
            enemyController.SwitchState(EnemyState.React);
        }
        else
        {
            LookToVector3(wayPoint);
        }
        if (enemyModel.agent.remainingDistance <= 0.2f && ! enemyModel.agent.pathPending)
        {
            //enemyModel.locationIndex = (enemyModel.locationIndex + 1) % enemyModel.locations.Count;
            enemyController.SwitchState(EnemyState.Idle);
        }
        
    }

    void GetNewWayPoint()
    {
        float randomX = Random.Range(-enemyModel.patrolRange, enemyModel.patrolRange);
        float randomZ = Random.Range(-enemyModel.patrolRange, enemyModel.patrolRange);

        Vector3 randomPoint = new Vector3(enemyModel.guardPos.x + randomX, enemyModel.transform.position.y, enemyModel.guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, enemyModel.patrolRange, 1) ? hit.position : enemyModel.transform.position;
    }

    public void MoveToPatrolPoints()
    {
        enemyModel.agent.destination = wayPoint;
    }
}
