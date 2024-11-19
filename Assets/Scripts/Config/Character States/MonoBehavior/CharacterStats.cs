using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class CharacterStats : MonoBehaviour
{

    public event Action<float, float> UpdateHealthBarOnAttack;

    public CharacterData_SO characterData;
    public CharacterData_SO templateData;
    public SkillConfig skillConfig;
    public SkillConfig templateSkillConfig;
    //角色名字
    public CharacterNameList characterName;
    #region Read from data_SO

    private void Awake()
    {
        if(templateData != null)
        {
            characterData = Instantiate(templateData);
        }
        if(templateSkillConfig != null)
        {
            skillConfig = Instantiate(templateSkillConfig);
        }
    }
    public float MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }

    public float CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }

    public float BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }

    public float CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }

    public float MaxSP
    {
        get { if (characterData!=null) return characterData.maxSP; else return 0; }
        set { characterData.maxSP = value; }
    }
    public float CurrentSP
    {
        get { if (characterData != null) return characterData.currentSP; else return 0; }
        set { characterData.currentSP = value; }
    }

    #endregion

    #region 受击
    public void TakeDamage(SkillConfig attacker,AttackInfo attackInfo)
    {
        float damage = attackInfo.attackDamageMultiple * (1 - CurrentDefence * 0.01f);
        //float damage = attacker.normalAttack[attacker.currentNormalAttackIndex - 1].attackDamageMultiple * (1 - CurrentDefence*0.01f);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        //TODO:Uppdate UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        //TODO:受击特效
    }
    #endregion

    #region Apply Data Change
    public void ApplyHealth(int amount)
    {
        if(CurrentHealth + amount <= MaxHealth)
        {
            CurrentHealth += amount;
        }
        else
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void ApplySP(int amount)
    {
        CurrentSP -= amount;
    }

    public void PlusSP(int amount)
    {
        if (CurrentSP + amount <= MaxSP)
        {
            CurrentSP += amount;
        }
        else
            CurrentSP = MaxSP;
    }

    public void ATKBUFF(SkillConfig attacker,float ATKmultiple,float durationTime)
    {
        AttackInfo[] attackMultiple = attacker.normalAttack;
        for (int i = 0; i < attacker.normalAttack.Length; i++)
        {
            attacker.normalAttack[i].attackDamageMultiple *= ATKmultiple;
        }
    }
    #endregion


}
