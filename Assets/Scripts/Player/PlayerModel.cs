using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public enum ModelFoot
{
    Right,Left
}

public class PlayerModel : MonoBehaviour, IHurt
{
    //动画控制器
    public Animator animator;
    //当前状态
    public PlayerState currentState;
    //重力
    public float gravity = -9.8f;
    //角色控制器
    public CharacterController characterController;
    // 技能配置文件
    public SkillConfig skillConfig;
    //大招Start镜头
    public GameObject bigSkillStartShot;
    //大招镜头
    public GameObject bigSkillShot;
    //动画信息
    private AnimatorStateInfo stateInfo;
    //武器列表
    public WeaponController[] weapons;
    //信息
    public CharacterStats characterStats;
    //切换状态脚本
    public SwitchState switchState;

    private int currentWeaponIndex;
    public  int currentVFXIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterStats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        characterStats.CurrentHealth = characterStats.MaxHealth;
        characterStats.CurrentDefence = characterStats.BaseDefence;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="enemyTagList"></param>
    /// <param name="onHitAction"></param>
    public void Init(List<string> enemyTagList)
    {
        for(int i=0;i<weapons.Length;i++)
        {
            weapons[i].Init(enemyTagList,OnHit);
        }
    }

    /// <summary>
    /// 命中事件
    /// </summary>
    private void OnHit(IHurt enemy)//攻击敌方时触发
    {
        string modelName;
        //TODO: Debug.Log(((Component)enemy).name);
        //Debug.Log(((Component)enemy).name);
        var currentEnemy = (Component)enemy;
        modelName = gameObject.name;
        modelName = modelName.Replace("(Clone)", "");
        AudioManager.INSTANCE.PlayAudio(modelName+"攻击受击音"+ skillConfig.currentNormalAttackIndex);
        

        //传递攻击类型
        if (currentEnemy.TryGetComponent<EnemyController>(out EnemyController enemyController))
        {
            enemyController.HurtEvent(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.damageDir,
                weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitType);
        }


        //产生hit特效
        Vector3 location = weapons[currentWeaponIndex].transform.position;
        Vector3 closestPoint = currentEnemy.GetComponent<Collider>().ClosestPoint(location);//获取碰撞位置
        Vector3 forword = characterStats.gameObject.transform.forward;
        /*
        VFXPoolManager.INSTANCE.SpawnHitVfx(currentEnemy.GetComponent<CharacterStats>().characterName, 
                                                                        PlayerController.INSTANCE.playerModel.characterStats.skillConfig.currentAttackInfo, 
                                                                        closestPoint, 
                                                                        forword,
                                                                        currentVFXIndex);
        */
        VFXPoolManager.INSTANCE.SpawnHitVfx(currentEnemy.GetComponent<CharacterStats>().characterName,
                                                                        weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo,
                                                                        closestPoint,
                                                                        forword,
                                                                        currentVFXIndex);

    }   
    
    /// <summary>
    /// 开启伤害检测
    /// </summary>
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
        currentWeaponIndex = weaponIndex;
        //weapons[weaponIndex].
    }

    public void SetVFXIndex(int index)
    {
        currentVFXIndex = index;
    }
    /// <summary>
    /// 关闭伤害检测
    /// </summary>
    public void StopHit(int weaponIndex)
    {
        weapons[weaponIndex].StopHit();
    }
    

    #region 动画状态
    public ModelFoot foot = ModelFoot.Right;

    public void Enter(Vector3 pos, Quaternion rot)
    {
        //强行移除退场逻辑
        MonoManager.INSTANCE.RemoveUpdateAction(OnExit);

        #region 设置角色出场位置
        //计算右方向向量
        Vector3 rightDirection = rot * Vector3.right;
        pos += rightDirection * 0.8f;

        //计算方向向量
        Vector3 backDirection = rot * Vector3.back;
        pos += backDirection;

        characterController.enabled = false;
        //characterController.Move(pos - transform.position);
        transform.SetPositionAndRotation(pos, rot);
        characterController.enabled = true;
        transform.rotation = rot;
        #endregion
    }

    public void ExitNormal()
    {
        animator.CrossFadeInFixedTime("SwitchOut_Normal", 0.1f);
        MonoManager.INSTANCE.AddUpdateAction(OnExit);
    }

    public void ExitSpecial()
    {
        MonoManager.INSTANCE.AddUpdateAction(OnExit);
    }


    //退场逻辑
    public void OnExit()
    {
        if(IsAnimationEnd())
        {
            gameObject.SetActive(false);
            MonoManager.INSTANCE.RemoveUpdateAction(OnExit);
        }
    }

    //动画结束
    public bool IsAnimationEnd()
    {
        #region 动画播放结束
        //刷新动画状态信息
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));

        #endregion
    }

    /// <summary>
    /// 迈出左脚
    /// </summary>
    public void SetOutLeftFoot()
    {
        foot = ModelFoot.Left;
    }

    /// <summary>
    /// 迈出右脚
    /// </summary>
    public void SetOutRightFoot()
    {
        foot = ModelFoot.Right;
    }
    #endregion

    public float ForwardAngle()
    {
        if (gameObject.transform.rotation.eulerAngles.y > 180)
        {
            return gameObject.transform.rotation.eulerAngles.y - 360;
        }
        else return gameObject.transform.rotation.eulerAngles.y;
    }

    private void OnDisable()
    {
        //重置普通攻击段数
        skillConfig.currentNormalAttackIndex = 1;
    }

    public void PlayVFX(string VFXname)
    {
        VFXPoolManager.INSTANCE.TryGetVFX(characterStats.characterName, VFXname);
    }

    
    #region 各种音频
    public void LeftFootAudio()
    {
        AudioManager.INSTANCE.PlayAudio("脚步声");
    }

    public void RightFootAudio()
    {
        AudioManager.INSTANCE.PlayAudio("脚步声1");
    }

    public void Evade1Audio()
    {
        AudioManager.INSTANCE.PlayAudio("闪避声1");
    }

    public void Evade2Audio()
    {
        AudioManager.INSTANCE.PlayAudio("闪避声2");
    }
    #endregion
}
