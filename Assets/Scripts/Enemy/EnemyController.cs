using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

public class EnemyController : MonoBehaviour, IHurt
{
    //���
    public Animator animator;
    public NavMeshAgent agent;
    public BehaviorTree tree;

    //�ű�
    public CharacterStats characterStats;
    //�����б�
    public WeaponController[] weapons;
    public Transform player;
    //���˱�ǩ�б�
    public List<string> enemyTagList;
    

    //����
    //������Ϣ
    private AnimatorStateInfo stateInfo;
    public int currentWeaponIndex = 0;
    //ת���ٶ�
    public float rotationSpeed = 8f;
    private DamageDir damageTrans;
    //�����¼�
    private Action<IHurt> onHitAction;
    public float hurtTimer;

    //״̬
    public bool isAttacking;
    public bool isHurt;
    public bool hurtTrigger;
    public bool isLookToPlayer;


    private void Awake()
    {
        player = null;
        characterStats = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        tree = GetComponent<BehaviorTree>();
    }

    private void Start()
    {
        Init(enemyTagList);
        isLookToPlayer = false;
        hurtTimer = 0f;
        AllEnemyController.INSTANCE.AddEnemyList(this);
    }

    private void Update()
    {
        agent.nextPosition = new Vector3(animator.rootPosition.x, animator.rootPosition.y + 1, animator.rootPosition.z);
        player = PlayerController.INSTANCE.playerModel.transform;
        if(isLookToPlayer)
        {
            LookToVector3(player.position, 8f);
        }
        HurtTrigger();
        HurtTimer();
    }

    public void HurtAnimationEvent(DamageDir dir, HitType hitType,PlayerModel player)
    {
        if ((Mathf.Abs(player.ForwardAngle() - ForwardAngle())) <= 70f)
        {
            damageTrans = DamageDir.Back;
        }
        else damageTrans = DamageDir.Front;
        
        animator.SetTrigger("Hit_Shake");
        if(!isAttacking)
        {
            hurtTrigger = true;
            HurtAnimation(dir, hitType);
            #region ģ����ת
            //������Һ͵��˵ķ���
            Vector3 direction = (player.transform.position - transform.position).normalized;
            switch (damageTrans)
            {
                case DamageDir.Front:
                    //����ģ���泯���
                    transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    break;
                case DamageDir.Back:
                    //����ģ�ͱ������
                    transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.z));
                    break;
            }
            #endregion
        }
        else
        {
            if(hitType != HitType.Light && hitType!=HitType.VeryLight)
            {
                hurtTrigger = true;
                HurtAnimation(dir, hitType);
                #region ģ����ת
                //������Һ͵��˵ķ���
                Vector3 direction = (player.transform.position - transform.position).normalized;
                switch (damageTrans)
                {
                    case DamageDir.Front:
                        //����ģ���泯���
                        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                        break;
                    case DamageDir.Back:
                        //����ģ�ͱ������
                        transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.z));
                        break;
                }
                #endregion
            }
        }
                    
        //��CharaStats�м����˺�
    }

    public void HurtAnimation(DamageDir dir, HitType hitType)
    {
        //Debug.Log(hitType);
        //Debug.Log(dir);
        switch (damageTrans)
        {
            case DamageDir.Front:
                switch (hitType)
                {
                    case HitType.VeryLight:
                        PlayAnimation("Hit_Shake", 0f);
                        break;
                    case HitType.Light:
                        //if( ! isAttacking)
                        //{
                            switch (dir)
                            {
                                case DamageDir.Left:
                                    PlayAnimation("Hit_L_Front_Left", 0f);
                                    break;
                                case DamageDir.Right:
                                    PlayAnimation("Hit_L_Front_Right",0f);
                                    break;
                                case DamageDir.Up:
                                    PlayAnimation("Hit_L_Front_Down", 0f);
                                    break;
                                case DamageDir.Down:
                                    PlayAnimation("Hit_L_Front_Up", 0f);
                                    break;
                            }
                        //}
                        break;
                    case HitType.Haven:
                        PlayAnimation("Hit_H_Front", 0f);
                        break;
                    case HitType.Fly:
                        {

                        }
                        break;
                }
                break;
            case DamageDir.Back:
                switch (hitType)
                {
                    case HitType.VeryLight:
                        PlayAnimation("Hit_Shake", 0f);
                        break;
                    case HitType.Light:
                        switch (dir)
                        {
                            case DamageDir.Left:
                                PlayAnimation("Hit_L_Back_Left", 0f);
                                break;
                            case DamageDir.Right:
                                PlayAnimation("Hit_L_Back_Right", 0f);
                                break;
                            case DamageDir.Up:
                                PlayAnimation("Hit_L_Back_Down", 0f);
                                break;
                            case DamageDir.Down:
                                PlayAnimation("Hit_L_Back_Up", 0f);
                                break;
                        }
                        break;
                    case HitType.Haven:
                        PlayAnimation("Hit_H_Back", 0f);
                        break;
                    case HitType.Fly:
                        {

                        }
                        break;
                }
                break;
        }
    }

    public float ForwardAngle()
    {
        if (gameObject.transform.rotation.eulerAngles.y > 180)
        {
            return gameObject.transform.rotation.eulerAngles.y - 360;
        }
        else return gameObject.transform.rotation.eulerAngles.y;
    }

    public void Init(List<string> enemyTagList)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Init(enemyTagList, OnHit);
        }
    }

    //���ез�ʱ����
    private void OnHit(IHurt enemy)
    {
        var currentPlayer = (Component)enemy;
        //Debug.Log(((Component)enemy).name);
        if (PlayerController.INSTANCE.isDodge)
        {
            Debug.Log("���ܳɹ�");
            PlayerController.INSTANCE.characterInfo[PlayerController.INSTANCE.currentModelIndex].GetComponent<PlayerModel>().PerfectDodgeEvent();
        }
        else
        {
            Debug.Log("����");
            PlayerController.INSTANCE.playerModel.HurtAnimationEvent(characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].damageDir,
               characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].hitType, this);

            //����hit��Ч
            Vector3 location = weapons[currentWeaponIndex].transform.position;
            Vector3 closestPoint = currentPlayer.GetComponent<Collider>().ClosestPoint(location);//��ȡ��ײλ��
            Vector3 forword = characterStats.gameObject.transform.forward;
            VFXPoolManager.INSTANCE.SpawnHitVfx(CharacterNameList.Enemy, characterStats.skillConfig.currentAttackInfo, closestPoint, forword);
        }
    }

    //�ܻ���ʱ���������ܻ�һ��ʱ���ʼ��������
    public void HurtTrigger()
    {
        if(hurtTrigger)
        {
            if(!isAttacking)
            {
                isHurt = true;
                hurtTimer = 0f;
            }
            
            hurtTrigger = false;
        }
    }

    public void HurtTimer()
    {
        if(isHurt)
        {
            hurtTimer += Time.deltaTime;
            if(hurtTimer >= 1f)
            {
                isHurt = false;
                hurtTimer = 0f;
            }
        }
    }

    public void LookToVector3(Vector3 target, float speed = 8f)
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
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQua, Time.deltaTime * speed);
    }



    public float GetDistance()
    {
        return Vector3.Distance(transform.position, player.position);
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

    /// <summary>
    /// �����˺����
    /// </summary>
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
    }
    /// <summary>
    /// �ر��˺����
    /// </summary>
    public void StopHit(int weaponIndex)
    {
        weapons[weaponIndex].StopHit();
    }

    /// <summary>
    /// ����ת��
    /// </summary>
    public void LookToPlayerStart()
    {
        isLookToPlayer = true;
    }
    /// <summary>
    /// �ر�ת��
    /// </summary>
    public void LookToPlayerStop()
    {
        isLookToPlayer = false;
    }

    public void ChangeAttackBool()
    {
        isAttacking = false;
    }
}
