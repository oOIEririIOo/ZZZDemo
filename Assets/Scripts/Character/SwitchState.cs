using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchState : MonoBehaviour, IStateMachineOwner
{
    public PlayerModel playerModel;
    public CharacterStats characterStats;

    //闪避计时器
    public float evadeTimer = 1f;

    //闪避计数器
    public int evadeCnt = 0;

    public virtual void Awake()
    {
        playerModel = GetComponent<PlayerModel>();
        characterStats = GetComponent<CharacterStats>();
    }
    public virtual void SwitichCharacterState(PlayerState playerState)
    {

    }
}
