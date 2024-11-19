using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyState
{
    Idle,Patrol,Chase,React,Turn,Attack,Dead,Hit
}

public class EnemyStateBase : StateBase
{
    EnemyState state;
    //记录当前状态进入的时间
    protected float animationPlayTime = 0f;
    //敌人控制器
    protected EnemyController enemyController;
    //敌人模型
    protected EnemyModel enemyModel;
    //动画信息
    protected AnimatorStateInfo stateInfo;

    public override void Enter()
    {
        animationPlayTime = 0f;
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Init(IStateMachineOwner owner)
    {
        enemyController = (EnemyController)owner;
        enemyModel = enemyController.enemyModel;
    }

    public override void LateUpdate()
    {
        
    }

    public override void UnInit()
    {
    }   

    public override void Update()
    {
        //状态进入计时
        animationPlayTime += Time.deltaTime;
    }

    //动画结束
    public bool IsAnimationEnd()
    {
        #region 动画播放结束
        //刷新动画状态信息
        stateInfo = enemyModel.animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !enemyModel.animator.IsInTransition(0));
        #endregion
    }

    public float NormalizedTime()
    {
        //刷新动画状态信息
        stateInfo = enemyModel.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }

    public void LookToTarget(Transform target)
    {
        if(target == null)
        {
            return;
        }
        //计算方向
        Vector3 direction = (target.position - enemyModel.transform.position).normalized;
        //模型面朝目标
        Quaternion targetQua = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        float angles = Mathf.Abs(targetQua.eulerAngles.y - enemyModel.transform.eulerAngles.y);
        enemyModel.transform.rotation = Quaternion.Slerp(enemyModel.transform.rotation, targetQua, Time.deltaTime * enemyModel.rotationSpeed);
    }

    public void LookToVector3(Vector3 target)
    {
        if (target == null)
        {
            return;
        }
        //计算方向
        Vector3 direction = (target - enemyModel.transform.position).normalized;
        //模型面朝目标
        Quaternion targetQua = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        float angles = Mathf.Abs(targetQua.eulerAngles.y - enemyModel.transform.eulerAngles.y);
        enemyModel.transform.rotation = Quaternion.Slerp(enemyModel.transform.rotation, targetQua, Time.deltaTime * enemyModel.rotationSpeed);
    }

    public void GetHit()
    {

    }
}
