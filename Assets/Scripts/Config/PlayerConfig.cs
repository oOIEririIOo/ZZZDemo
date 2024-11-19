using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家配置信息
/// </summary>
[CreateAssetMenu(menuName ="Config/Player Config")]
public class PlayerConfig : ScriptableObject
{
    //玩家配队
    public GameObject[] models;
}
