using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
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
    //QTE相机
    public Transform QTECameraPoint;
    //动画信息
    private AnimatorStateInfo stateInfo;
    //武器列表
    public WeaponController[] weapons;
    //信息
    public CharacterStats characterStats;
    //切换状态脚本
    public SwitchState switchState;
    //闪避触发器
    public Collider dodgeColl;
    //材质
    public SkinnedMeshRenderer[] meshRenderer;
    //受伤类型
    public DamageDir damageTrans;
    public HitType hitType;
    //死亡bool
    public bool isDead;
    public bool cantSwitich;
    public bool isQTE;

    private int currentWeaponIndex;
    public  int currentVFXIndex = 0;
    //public int hitBoxIndex;

    public Transform dodgeEffPos;
    public bool parryTiming;


    private Coroutine OutLineBlue;
    private Coroutine OutLineOrange;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterStats = GetComponent<CharacterStats>();
        
    }

    private void Start()
    {
        /*
        characterStats.CurrentHealth = characterStats.MaxHealth;
        characterStats.CurrentDefence = characterStats.BaseDefence;
        characterStats.CurrentSP = characterStats.MaxSP;
        */
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
    private void OnHit(IHurt enemy)//攻击命中敌方时触发
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
            enemyController.HurtAnimationEvent(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].damageDir,
                weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].hitType,
                weapons[currentWeaponIndex].characterStats.GetComponent<PlayerModel>());
        }
        //传递敌人信息给镜头特效
        CameraHitFeel.INSTANCE.GetCurrentEnemyAnimation(enemyController);
        //钝帧
        float pauseFrameTime = weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].pauseFrameTime;
        CameraHitFeel.INSTANCE.PauseFrame(pauseFrameTime);

        //产生hit特效
        Vector3 location = weapons[currentWeaponIndex].transform.position;
        Vector3 closestPoint = currentEnemy.GetComponent<Collider>().bounds.ClosestPoint(location);//获取碰撞位置
        Vector3 forword = characterStats.gameObject.transform.forward;
        VFXPoolManager.INSTANCE.SpawnHitVfx(currentEnemy.GetComponent<CharacterStats>().characterName,
                                                                        weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo,
                                                                        closestPoint,
                                                                        forword);

        characterStats.AddSP(2);
        currentEnemy.GetComponent<CharacterStats>().TakeDamage(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo);
        if(enemyController.isStun == false)
            currentEnemy.GetComponent<CharacterStats>().AddStun(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo);


        //每次攻击检查目标的失衡值，若失衡值满了开启连携
        if ((characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].canQTE 
            && enemyController.GetComponent<CharacterStats>().CurrentStun == enemyController.GetComponent<CharacterStats>().MaxStun && PlayerController.INSTANCE.QTETarget == null))
        {
            PlayerController.INSTANCE.QTETarget = enemyController;
            QTEManager.INSTANCE.canQTE = true;
            QTEStartEvent();
        }
    }   

    //QTEStart该函数有两个触发情况：一是可触发连携的攻击命中 二是连携动画事件触发
    public void QTEStartEvent()
    {
        if(QTEManager.INSTANCE.QTECount == PlayerController.INSTANCE.controllableModels.Count)
        {
            QTEManager.INSTANCE.CancelQTE();
        }
        //enemyController.isStun = true;
        if (QTEManager.INSTANCE.canQTE)
        {
            CameraManager.INSTANCE.virtualCameraComponent.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 1.5f;
            CameraManager.INSTANCE.virtualCameraComponent.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = -0.5f;
            CameraHitFeel.INSTANCE.QTEStart(5, 0.1f);
        }
        
    }

    public void PerfectDodgeEvent()
    {
        PlayVFX("DodgeEff", dodgeEffPos);
        
        if(OutLineBlue != null)
        {
            StopCoroutine(OutLineBlue);
        }
        OutLineBlue = StartCoroutine(OutLineBlueIE(1.5f, 1.6f));
        CameraHitFeel.INSTANCE.SlowMotion(0.1f, 0.25f);
    }
    
    /// <summary>
    /// 开启伤害检测
    /// </summary>
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
        currentWeaponIndex = weaponIndex;
        characterStats.skillConfig.currentAttackInfo.hitIndex++;
        //hitBoxIndex++;
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
        /*
        //计算右方向向量
        Vector3 rightDirection = rot * Vector3.right;
        pos += rightDirection * 0.8f;

        //计算方向向量
        Vector3 backDirection = rot * Vector3.back;
        pos += backDirection;
        */
        characterController.enabled = false;
        //characterController.Move(pos - transform.position);
        transform.SetPositionAndRotation(pos, rot);
        characterController.enabled = true;
        transform.rotation = rot;
        #endregion
        OutLineBlack();
    }

    

    public void GetParryTarget(EnemyController enemy)
    {
        PlayerController.INSTANCE.parryTarget = enemy;
    }

    public void ParryEvent()
    {
        PlayerController.INSTANCE.parryTarget.beParring = true;
        PlayerController.INSTANCE.parryTarget.GetComponent<CharacterStats>().AddStun(characterStats.skillConfig.currentAttackInfo, 20f);
        CameraHitFeel.INSTANCE.SlowMotion(0.35f, 0.01f);
        if (OutLineOrange != null)
        {
            StopCoroutine(OutLineOrange);
        }
        OutLineOrange = StartCoroutine(OutLineOrangeIE(1.5f, 1.6f));
        PlayVFX("ParryEff");
        CameraShakeOnAnim(2f);
        Debug.Log("ParryEvent");
        
        AllEnemyController.INSTANCE.ClearParryList();
        
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
            cantSwitich = false;
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

    public void CameraShakeOnAnim(float force)
    {
        CameraHitFeel.INSTANCE.CameraShake(force);
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

    public void HurtAnimationEvent(DamageDir dir, HitType hitType,EnemyController enemy)
    {
        if ((Mathf.Abs(ForwardAngle() - enemy.ForwardAngle())) <= 75f)
        {
            damageTrans = DamageDir.Back;
        }
        else damageTrans = DamageDir.Front;

        //animator.SetTrigger("Hit_Shake");
        HitAnimation();
        //计算玩家和敌人的方向
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        switch(damageTrans)
        {
            case DamageDir.Front:
                //玩家模型面朝敌人
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                break;
            case DamageDir.Back:
                //玩家模型背朝敌人
                transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.z));
                break;
        }
        
        //在CharaStats中计算伤害
    }

    public void HitAnimation()
    {
        PlayerController.INSTANCE.SwitchState(PlayerState.Hit);
    }


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

    public void PlayVFX(string VFXname,Transform pos)
    {
        VFXPoolManager.INSTANCE.TryGetVFX(characterStats.characterName, VFXname,pos);
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

    private void OutLineBlack()
    {
        foreach (var mR in meshRenderer)
        {
            foreach (var name in mR.materials)
            {
                if (name.name == "OutLineBlue (Instance)" || name.name == "OutLineOrange (Instance)")
                {
                    name.SetFloat("_OutlineWidth", 0.6f);
                }
            }
            //mt.materials[0].SetFloat("_OutlineWidth", currentIndex);
        }
    }
    IEnumerator OutLineBlueIE(float time,float index)
    {
        float currentIndex = index;
        foreach (var mR in meshRenderer)
        {
            foreach(var name in mR.materials)
            {
                if(name.name == "OutLineBlue (Instance)")
                {
                    name.SetFloat("_OutlineWidth", currentIndex);
                }
            }
            //mt.materials[0].SetFloat("_OutlineWidth", currentIndex);
        }
        yield return new WaitForSeconds(time);
        float minValue = 0.1f;
        while (Mathf.Abs(currentIndex - 0.6f) > minValue)
        {
            currentIndex = Mathf.Lerp(currentIndex, 0.6f, Time.deltaTime * 2.5f);
            foreach (var mR in meshRenderer)
            {
                foreach (var name in mR.materials)
                {
                    if (name.name == "OutLineBlue (Instance)")
                    {
                        name.SetFloat("_OutlineWidth", currentIndex);
                    }
                }
                //mt.materials[0].SetFloat("_OutlineWidth", currentIndex);
            }
            yield return null;
        }
        
        currentIndex = 0.6f;
        foreach (var mR in meshRenderer)
        {
            foreach (var name in mR.materials)
            {
                if (name.name == "OutLineBlue (Instance)")
                {
                    name.SetFloat("_OutlineWidth", currentIndex);
                }
            }
            //mt.materials[0].SetFloat("_OutlineWidth", currentIndex);
        }
    }
    IEnumerator OutLineOrangeIE(float time, float index)
    {
        float currentIndex = index;
        foreach (var mR in meshRenderer)
        {
            foreach (var name in mR.materials)
            {
                if (name.name == "OutLineOrange (Instance)")
                {
                    name.SetFloat("_OutlineWidth", currentIndex);
                }
            }
            //mt.materials[0].SetFloat("_OutlineWidth", currentIndex);
        }
        yield return new WaitForSeconds(time);
        float minValue = 0.1f;
        while (Mathf.Abs(currentIndex - 0.6f) > minValue)
        {
            currentIndex = Mathf.Lerp(currentIndex, 0.6f, Time.deltaTime * 2.5f);
            foreach (var mR in meshRenderer)
            {
                foreach (var name in mR.materials)
                {
                    if (name.name == "OutLineOrange (Instance)")
                    {
                        name.SetFloat("_OutlineWidth", currentIndex);
                    }
                }
                //mt.materials[0].SetFloat("_OutlineWidth", currentIndex);
            }
            yield return null;
        }

        currentIndex = 0.6f;
        foreach (var mR in meshRenderer)
        {
            foreach (var name in mR.materials)
            {
                if (name.name == "OutLineOrange (Instance)")
                {
                    name.SetFloat("_OutlineWidth", currentIndex);
                }
            }
            //mt.materials[0].SetFloat("_OutlineWidth", currentIndex);
        }
    }
}
