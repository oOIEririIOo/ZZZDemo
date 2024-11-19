using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VFXItemData;

/// <summary>
/// ��������
/// </summary>
[CreateAssetMenu(menuName = "Config/Skill")]
public class SkillConfig : ScriptableObject
{
    //��ǰ��ͨ��������
    public int currentNormalAttackIndex = 1;

    //����ÿ�ε��˺�����
    public AttackInfo[] normalAttack;

    //��ǰ���ܷ�֧����
    public int currentBranchIndex = 1;
  
    //���ܷ�֧ÿ�ε��˺�����
    public AttackInfo[] branch;

    //��ǰ������Ϣ
    public AttackInfo currentAttackInfo;
}



[System.Serializable]
public class AttackInfo
{
    public DamageType damageType;
    public float attackDamageMultiple;
    public int hitCont;
    public EffectItem[] hitVFX;
    public HitType hitType;
    public DamageDir damageDir;
    public int SP;
    //���л���ʧ��ֵ������ֵ
}

public enum HitType
{
    Light,Haven,Fly
}
public enum DamageDir
{
    Left, Right, Up, Down, Front, Back
}

public enum DamageType
{
    Normal,Ice,Fire,Lightning,Aether
}
