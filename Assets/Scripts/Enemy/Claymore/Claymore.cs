using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

public class Claymore : MonoBehaviour, IHurt
{
    public CharacterStats characterStats;
    public Transform player;
    //动画控制器
    public Animator animator;
    //动画信息
    private AnimatorStateInfo stateInfo;
    //转向速度
    public float rotationSpeed = 8f;

    public NavMeshAgent agent;


    //武器列表
    public WeaponController[] weapons;

    //命中事件
    private Action<IHurt> onHitAction;

    //敌人标签列表
    public List<string> enemyTagList;

    public BehaviorTree tree;

    private void Awake()
    {
        player = null;
        characterStats = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        tree = GetComponent<BehaviorTree>();
    }

    private void Update()
    {
        agent.nextPosition = new Vector3(animator.rootPosition.x, animator.rootPosition.y+1, animator.rootPosition.z);
        player = PlayerController.INSTANCE.playerModel.transform;
    }

    public void LookToVector3(Vector3 target)
    {
        if (target == null)
        {
            return;
        }
        //计算方向
        Vector3 direction = (target - transform.position).normalized;
        //模型面朝目标
        Quaternion targetQua = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        float angles = Mathf.Abs(targetQua.eulerAngles.y - transform.eulerAngles.y);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQua, Time.deltaTime * rotationSpeed);
    }

    public void PlayAnimation(string animationName, float fixedTransitionDuration = 0.25f)
    {

            animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration);
   
        
    }

    public float NormalizedTime()
    {
        //刷新动画状态信息
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }

    public bool IsAnimationEnd()
    {
        #region 动画播放结束
        //刷新动画状态信息
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
        #endregion
    }
}
