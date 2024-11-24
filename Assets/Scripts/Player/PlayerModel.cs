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
    //��������
    public DamageDir damageTrans;
    public HitType hitType;

    private int currentWeaponIndex;
    public  int currentVFXIndex = 0;
    //public int hitBoxIndex;

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


        //����hit��Ч
        Vector3 location = weapons[currentWeaponIndex].transform.position;
        Vector3 closestPoint = currentEnemy.GetComponent<Collider>().ClosestPoint(location);//��ȡ��ײλ��
        Vector3 forword = characterStats.gameObject.transform.forward;
        VFXPoolManager.INSTANCE.SpawnHitVfx(currentEnemy.GetComponent<CharacterStats>().characterName,
                                                                        weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo,
                                                                        closestPoint,
                                                                        forword);
                                                                       

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
        //�����ҷ�������
        Vector3 rightDirection = rot * Vector3.right;
        pos += rightDirection * 0.8f;

        //���㷽������
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


    //�˳��߼�
    public void OnExit()
    {
        if(IsAnimationEnd())
        {
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
}
