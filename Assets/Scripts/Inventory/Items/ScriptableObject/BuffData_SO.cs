using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff",menuName ="Buff/Buff  Data")]
public class BuffData_SO : ScriptableObject
{
    public float ATKmultiple;
    public float DEFmultiple;
    public float durationTime;

    public void BUFFOn(SkillConfig user, CharacterStats userData)
    {
        for (int i = 0; i < user.normalAttack.Length; i++)
        {
            user.normalAttack[i].attackDamageMultiple *= ATKmultiple;
        }
        userData.CurrentDefence *= DEFmultiple;
    }

    public void BUFFOff(SkillConfig user, CharacterStats userData)
    {
        for (int i = 0; i < user.normalAttack.Length; i++)
        {
            user.normalAttack[i].attackDamageMultiple /= ATKmultiple;
        }
        userData.CurrentDefence /= DEFmultiple;
    }
}
