using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VFXItemData;

/// <summary>
/// 技能配置
/// </summary>
[CreateAssetMenu(menuName = "Config/Skill")]
public class SkillConfig : ScriptableObject
{
    //当前普通攻击段数
    public int currentNormalAttackIndex = 1;

    //攻击每段的伤害倍率
    public AttackInfo[] normalAttack;

    //当前技能分支段数
    public int currentBranchIndex = 1;
  
    //技能分支每段的伤害倍率
    public AttackInfo[] branch;

    //当前攻击信息
    public AttackInfo currentAttackInfo;
}



[System.Serializable]
public class AttackInfo
{
    public HitInfo[] hitInfo;
    public int hitIndex;
    public int SP;
    /*
    public DamageType damageType;
    public float attackDamageMultiple;
    public int hitCont;
    public EffectItem[] hitVFX;
    public HitType hitType;
    public DamageDir damageDir;
    public int SP;
    */
    //还有击打失衡值，韧性值
}

[System.Serializable]
public class HitInfo
{
    public DamageType damageType;
    public float attackDamageMultiple;
    public EffectItem hitVFX;
    public HitType hitType;
    public DamageDir damageDir;
    public float pauseFrameTime;
    public bool canInterrupt;
}

public enum HitType
{
    Light,Haven,Fly,VeryLight
}
public enum DamageDir
{
    Left, Right, Up, Down, Front, Back
}

public enum DamageType
{
    Normal,Ice,Fire,Lightning,Aether
}
