using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchState : MonoBehaviour, IStateMachineOwner
{
    public PlayerModel playerModel;
    public CharacterStats characterStats;

    //���ܼ�ʱ��
    public float evadeTimer = 1f;

    //���ܼ�����
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
