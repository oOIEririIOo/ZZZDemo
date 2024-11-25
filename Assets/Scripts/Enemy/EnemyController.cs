using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

public class EnemyController : MonoBehaviour, IHurt
{
    //组件
    public Animator animator;
    public NavMeshAgent agent;
    public BehaviorTree tree;

    //脚本
    public CharacterStats characterStats;
    //武器列表
    public WeaponController[] weapons;
    public Transform player;
    //敌人标签列表
    public List<string> enemyTagList;
    

    //参数
    //动画信息
    private AnimatorStateInfo stateInfo;
    public int currentWeaponIndex = 0;
    //转向速度
    public float rotationSpeed = 8f;
    private DamageDir damageTrans;
    //命中事件
    private Action<IHurt> onHitAction;
    public float hurtTimer;

    //状态
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
            #region 模型旋转
            //计算玩家和敌人的方向
            Vector3 direction = (player.transform.position - transform.position).normalized;
            switch (damageTrans)
            {
                case DamageDir.Front:
                    //敌人模型面朝玩家
                    transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    break;
                case DamageDir.Back:
                    //敌人模型背朝玩家
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
                #region 模型旋转
                //计算玩家和敌人的方向
                Vector3 direction = (player.transform.position - transform.position).normalized;
                switch (damageTrans)
                {
                    case DamageDir.Front:
                        //敌人模型面朝玩家
                        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                        break;
                    case DamageDir.Back:
                        //敌人模型背朝玩家
                        transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.z));
                        break;
                }
                #endregion
            }
        }
                    
        //在CharaStats中计算伤害
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

    //命中敌方时触发
    private void OnHit(IHurt enemy)
    {
        var currentPlayer = (Component)enemy;
        //Debug.Log(((Component)enemy).name);
        if (PlayerController.INSTANCE.isDodge)
        {
            Debug.Log("闪避成功");
            PlayerController.INSTANCE.characterInfo[PlayerController.INSTANCE.currentModelIndex].GetComponent<PlayerModel>().PerfectDodgeEvent();
        }
        else
        {
            Debug.Log("命中");
            PlayerController.INSTANCE.playerModel.HurtAnimationEvent(characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].damageDir,
               characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].hitType, this);

            //产生hit特效
            Vector3 location = weapons[currentWeaponIndex].transform.position;
            Vector3 closestPoint = currentPlayer.GetComponent<Collider>().ClosestPoint(location);//获取碰撞位置
            Vector3 forword = characterStats.gameObject.transform.forward;
            VFXPoolManager.INSTANCE.SpawnHitVfx(CharacterNameList.Enemy, characterStats.skillConfig.currentAttackInfo, closestPoint, forword);
        }
    }

    //受击计时器，连续受击一定时间后开始发动攻击
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
        //计算方向
        Vector3 direction = (target - transform.position).normalized;
        //模型面朝目标
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

    /// <summary>
    /// 开启伤害检测
    /// </summary>
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
    }
    /// <summary>
    /// 关闭伤害检测
    /// </summary>
    public void StopHit(int weaponIndex)
    {
        weapons[weaponIndex].StopHit();
    }

    /// <summary>
    /// 开启转向
    /// </summary>
    public void LookToPlayerStart()
    {
        isLookToPlayer = true;
    }
    /// <summary>
    /// 关闭转向
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
