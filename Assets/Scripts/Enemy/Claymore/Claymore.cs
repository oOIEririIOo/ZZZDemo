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
    public Transform target;
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
        target = null;
        characterStats = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        tree = GetComponent<BehaviorTree>();
    }

    public void PlayAnimation(string animationName, float fixedTransitionDuration = 0.25f)
    {
        if( ! animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration);
        }
        
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
