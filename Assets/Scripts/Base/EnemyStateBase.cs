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
    //��¼��ǰ״̬�����ʱ��
    protected float animationPlayTime = 0f;
    //���˿�����
    protected EnemyController enemyController;
    //����ģ��
    protected EnemyModel enemyModel;
    //������Ϣ
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
        //״̬�����ʱ
        animationPlayTime += Time.deltaTime;
    }

    //��������
    public bool IsAnimationEnd()
    {
        #region �������Ž���
        //ˢ�¶���״̬��Ϣ
        stateInfo = enemyModel.animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !enemyModel.animator.IsInTransition(0));
        #endregion
    }

    public float NormalizedTime()
    {
        //ˢ�¶���״̬��Ϣ
        stateInfo = enemyModel.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }

    public void LookToTarget(Transform target)
    {
        if(target == null)
        {
            return;
        }
        //���㷽��
        Vector3 direction = (target.position - enemyModel.transform.position).normalized;
        //ģ���泯Ŀ��
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
        //���㷽��
        Vector3 direction = (target - enemyModel.transform.position).normalized;
        //ģ���泯Ŀ��
        Quaternion targetQua = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        float angles = Mathf.Abs(targetQua.eulerAngles.y - enemyModel.transform.eulerAngles.y);
        enemyModel.transform.rotation = Quaternion.Slerp(enemyModel.transform.rotation, targetQua, Time.deltaTime * enemyModel.rotationSpeed);
    }

    public void GetHit()
    {

    }
}
