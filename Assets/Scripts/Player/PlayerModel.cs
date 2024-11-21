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
    private void OnHit(IHurt enemy)//�����з�ʱ����
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
            enemyController.HurtEvent(weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.damageDir,
                weapons[currentWeaponIndex].characterStats.skillConfig.currentAttackInfo.hitType);
        }


        //����hit��Ч
        Vector3 location = weapons[currentWeaponIndex].transform.position;
        Vector3 closestPoint = currentEnemy.GetComponent<Collider>().ClosestPoint(location);//��ȡ��ײλ��
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
    /// �����˺����
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
