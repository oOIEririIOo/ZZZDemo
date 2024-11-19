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
    //����������
    public Animator animator;
    //������Ϣ
    private AnimatorStateInfo stateInfo;
    //ת���ٶ�
    public float rotationSpeed = 8f;

    public NavMeshAgent agent;

    //�����б�
    public WeaponController[] weapons;

    //�����¼�
    private Action<IHurt> onHitAction;

    //���˱�ǩ�б�
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
        #region �������Ž���
        //ˢ�¶���״̬��Ϣ
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));
        #endregion
    }
}
