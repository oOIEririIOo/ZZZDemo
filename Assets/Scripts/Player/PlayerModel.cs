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
    //����������
    public Animator animator;
    //��ǰ״̬
    public PlayerState currentState;
    //����
    public float gravity = -9.8f;
    //��ɫ������
    public CharacterController characterController;
    // ���������ļ�
    public SkillConfig skillConfig;
    //����Start��ͷ
    public GameObject bigSkillStartShot;
    //���о�ͷ
    public GameObject bigSkillShot;
    //QTE���
    public Transform QTECameraPoint;
    //������Ϣ
    private AnimatorStateInfo stateInfo;
    //�����б�
    public WeaponController[] weapons;
    //��Ϣ
    public CharacterStats characterStats;
    //�л�״̬�ű�
    public SwitchState switchState;
    //���ܴ�����
    public Collider dodgeColl;
    //����
    public SkinnedMeshRenderer[] meshRenderer;
    //��������
    public DamageDir damageTrans;
    public HitType hitType;
    //����bool
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
    /// ��ʼ��
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
    /// �����¼�
    /// </summary>
    private void OnHit(IHurt enemy)//�������ез�ʱ����
    {
        string modelName;
        //TODO: Debug.Log(((Component)enemy).name);
        //Debug.Log(((Component)enemy).name);
        var currentEnemy = (Component)enemy;
        
        modelName = gameObject.name;
        modelName = modelName.Replace("(Clone)", "");
        AudioManager.INSTANCE.PlayAudio(modelName+"�����ܻ���"+ skillConfig.currentNormalAttackIndex);
        

        //���ݹ�������
        if (currentEnemy.TryGetComponent<EnemyController>(out EnemyController enemyController))
        {
            enemyController.HurtAnimationEvent(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].damageDir,
                weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].hitType,
                weapons[currentWeaponIndex].characterStats.GetComponent<PlayerModel>());
        }
        //���ݵ�����Ϣ����ͷ��Ч
        CameraHitFeel.INSTANCE.GetCurrentEnemyAnimation(enemyController);
        //��֡
        float pauseFrameTime = weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].pauseFrameTime;
        CameraHitFeel.INSTANCE.PauseFrame(pauseFrameTime);

        //����hit��Ч
        Vector3 location = weapons[currentWeaponIndex].transform.position;
        Vector3 closestPoint = currentEnemy.GetComponent<Collider>().bounds.ClosestPoint(location);//��ȡ��ײλ��
        Vector3 forword = characterStats.gameObject.transform.forward;
        VFXPoolManager.INSTANCE.SpawnHitVfx(currentEnemy.GetComponent<CharacterStats>().characterName,
                                                                        weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo,
                                                                        closestPoint,
                                                                        forword);

        characterStats.AddSP(2);
        currentEnemy.GetComponent<CharacterStats>().TakeDamage(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo);
        if(enemyController.isStun == false)
            currentEnemy.GetComponent<CharacterStats>().AddStun(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo);


        //ÿ�ι������Ŀ���ʧ��ֵ����ʧ��ֵ���˿�����Я
        if ((characterStats.skillConfig.currentAttackInfo.hitInfo[characterStats.skillConfig.currentAttackInfo.hitIndex].canQTE 
            && enemyController.GetComponent<CharacterStats>().CurrentStun == enemyController.GetComponent<CharacterStats>().MaxStun && PlayerController.INSTANCE.QTETarget == null))
        {
            PlayerController.INSTANCE.QTETarget = enemyController;
            QTEManager.INSTANCE.canQTE = true;
            QTEStartEvent();
        }
    }   

    //QTEStart�ú������������������һ�ǿɴ�����Я�Ĺ������� ������Я�����¼�����
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
    /// �����˺����
    /// </summary>
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
        currentWeaponIndex = weaponIndex;
        characterStats.skillConfig.currentAttackInfo.hitIndex++;
        //hitBoxIndex++;
    }

    /// <summary>
    /// �ر��˺����
    /// </summary>
    public void StopHit(int weaponIndex)
    {
        weapons[weaponIndex].StopHit();
    }
    

    #region ����״̬
    public ModelFoot foot = ModelFoot.Right;

    public void Enter(Vector3 pos, Quaternion rot)
    {
        //ǿ���Ƴ��˳��߼�
        MonoManager.INSTANCE.RemoveUpdateAction(OnExit);

        #region ���ý�ɫ����λ��
        /*
        //�����ҷ�������
        Vector3 rightDirection = rot * Vector3.right;
        pos += rightDirection * 0.8f;

        //���㷽������
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


    //�˳��߼�
    public void OnExit()
    {
        if(IsAnimationEnd())
        {
            cantSwitich = false;
            gameObject.SetActive(false);
            MonoManager.INSTANCE.RemoveUpdateAction(OnExit);
        }
    }

    //��������
    public bool IsAnimationEnd()
    {
        #region �������Ž���
        //ˢ�¶���״̬��Ϣ
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0));

        #endregion
    }

    public void CameraShakeOnAnim(float force)
    {
        CameraHitFeel.INSTANCE.CameraShake(force);
    }

    /// <summary>
    /// �������
    /// </summary>
    public void SetOutLeftFoot()
    {
        foot = ModelFoot.Left;
    }

    /// <summary>
    /// �����ҽ�
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
        //������Һ͵��˵ķ���
        Vector3 direction = (enemy.transform.position - transform.position).normalized;
        switch(damageTrans)
        {
            case DamageDir.Front:
                //���ģ���泯����
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                break;
            case DamageDir.Back:
                //���ģ�ͱ�������
                transform.rotation = Quaternion.LookRotation(new Vector3(-direction.x, 0, -direction.z));
                break;
        }
        
        //��CharaStats�м����˺�
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
        //������ͨ��������
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


    #region ������Ƶ
    public void LeftFootAudio()
    {
        AudioManager.INSTANCE.PlayAudio("�Ų���");
    }

    public void RightFootAudio()
    {
        AudioManager.INSTANCE.PlayAudio("�Ų���1");
    }

    public void Evade1Audio()
    {
        AudioManager.INSTANCE.PlayAudio("������1");
    }

    public void Evade2Audio()
    {
        AudioManager.INSTANCE.PlayAudio("������2");
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
