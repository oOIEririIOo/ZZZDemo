using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public float maxHealth;
    public float currentHealth;
    public float baseDefence;
    public float currentDefence;
    public float maxSP;
    public float currentSP;
}
