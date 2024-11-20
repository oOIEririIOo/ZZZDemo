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
        //���㷽��
        Vector3 direction = (target - transform.position).normalized;
        //ģ���泯Ŀ��
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
        //ˢ�¶���״̬��Ϣ
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
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
