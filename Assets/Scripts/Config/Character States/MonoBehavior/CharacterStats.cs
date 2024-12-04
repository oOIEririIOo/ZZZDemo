using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{

    public event Action<float, float> UpdateHealthBarOnAttack;
    public event Action<float, float> UpdateStunBarOnAttack;

    public CharacterData_SO characterData;
    public CharacterData_SO templateData;
    public SkillConfig skillConfig;
    public SkillConfig templateSkillConfig;
    //��ɫ����
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

    public float MaxStun
    {
        get { if (characterData != null) return characterData.maxStun; else return 0; }
        set { characterData.maxStun = value; }
    }

    public float CurrentStun
    {
        get { if (characterData != null) return characterData.currentStun; else return 0; }
        set { characterData.currentStun = value; }
    }

    public Sprite Icon
    {
        get { if (characterData != null) return characterData.icon; else return null; }
        
    }

    #endregion

    #region �ܻ�
    public void TakeDamage(AttackInfo attackInfo)
    {
        float damage = attackInfo.hitInfo[attackInfo.hitIndex].attackDamageMultiple * (1 - CurrentDefence * 0.01f);
        //float damage = attacker.normalAttack[attacker.currentNormalAttackIndex - 1].attackDamageMultiple * (1 - CurrentDefence*0.01f);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        //TODO:Uppdate UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth,MaxHealth);
        Debug.Log(this.gameObject.name+ "�ܵ�"+damage+"�˺�");
        //TODO:�ܻ���Ч
    }
    #endregion

    #region ����ʧ��ֵ
    public void AddStun(AttackInfo attackInfo)
    {
        float stun = attackInfo.hitInfo[attackInfo.hitIndex].stun;
        CurrentStun = Mathf.Min(CurrentStun + stun, MaxStun);
        UpdateStunBarOnAttack?.Invoke(CurrentStun, MaxStun);
    }

    public void AddStun(AttackInfo attackInfo,float index)//�������õ����ط���
    {
        float stun = index;
        CurrentStun = Mathf.Min(CurrentStun + stun, MaxStun);
        UpdateStunBarOnAttack?.Invoke(CurrentStun, MaxStun);
    }
    #endregion

    #region ����ʧ��ֵ

    Coroutine RemoveStunCoroutine;
    public void RemoveStun()
    {
        if(RemoveStunCoroutine != null)
        {
            StopCoroutine(RemoveStunCoroutine);
        }
        RemoveStunCoroutine = StartCoroutine(RemoveStunEff());
    }

    IEnumerator RemoveStunEff()
    {
        while(CurrentStun >= 1f)
        {
            CurrentStun -= 2f;
            UpdateStunBarOnAttack?.Invoke(CurrentStun, MaxStun);
            yield return new WaitForSeconds(0.1f);
        }
        CurrentStun = 0f;
        if(TryGetComponent<EnemyController>(out EnemyController enemyController))
        {
            enemyController.isStun = false;
        }
        UpdateStunBarOnAttack?.Invoke(CurrentStun, MaxStun);
    }
    #endregion

    #region ����SP
    public void AddSP(float index)
    {
        CurrentSP = Mathf.Min(CurrentSP + index, MaxSP);
        PlayerStatsUIManager.INSTANCE.UpdatePlayersUI();
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
        PlayerStatsUIManager.INSTANCE.UpdatePlayersUI();
    }

    public void ApplySP(int amount)
    {
        CurrentSP -= amount;
        PlayerStatsUIManager.INSTANCE.UpdatePlayersUI();
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
            //attacker.normalAttack[i].attackDamageMultiple *= ATKmultiple;
        }
    }
    #endregion


}
