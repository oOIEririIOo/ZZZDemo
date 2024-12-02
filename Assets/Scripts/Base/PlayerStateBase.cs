using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public enum PlayerState
{
    Idle,Idle_AFK ,
    Walk ,Run, RunEnd, TurnBack,TurnBack_End, 
    Evade_Front, Evade_Front_End, Evade_Back, Evade_Back_End,
    NormalAttack, NormalAttackEnd, Attack_Rush, Attack_Rush_End,
    Counter,Counter_End,
    Branch,Branch_End,SpBranch,SpBranch_End,
    BigSkillStart, BigSkill, BigSkillEnd, 
    SwitchInNormal,Parry,ParryEnd,
    Hit,
    Pause,


    Unagi_HavenAttack,Unagi_HavenAttackEnd,Unagi_BranchStart,Unagi_HoldBranch,Unagi_HoldBranchEnd,
    Anbi_PerfectAttack,Anbi_PerfectAttack_End,Anbi_PerfectBranch, Anbi_PerfectBranch_End, Anbi_PerfectSPBranch, Anbi_PerfectSPBranch_End
}

public class PlayerStateBase : StateBase
{
    //���״̬
    PlayerState state;
    //��ҿ�����
    protected PlayerController playerController;
    //���ģ��
    protected PlayerModel playerModel;
    //������Ϣ
    private AnimatorStateInfo stateInfo;
    //��¼��ǰ״̬�����ʱ��
    protected float animationPlayTime = 0f;
    //�Ƿ����˺�������Ŷ���
    protected bool isContinuePlay;


    public override void Init(IStateMachineOwner owner)
    {
        playerController = (PlayerController)owner;
        playerModel = playerController.playerModel;
    }
    public override void Enter()
    {
        animationPlayTime = 0f;
        
    }

    public override void Exit()
    {
        isContinuePlay = false;
    }

    public override void FixedUpdate()
    {
        
    }

    

    public override void LateUpdate()
    {
      
    }

    public override void UnInit()
    {

    }

    public override void Update()
    {
        //ʩ������
        playerModel.characterController.Move(new Vector3(0, playerModel.gravity * Time.deltaTime, 0));

        if(playerController.mouseOpen)
        {
            playerController.SwitchState(PlayerState.Pause);
        }

        //״̬�����ʱ
        animationPlayTime += Time.deltaTime;

        #region ����ɫ�л�
        if(playerModel.currentState != PlayerState.BigSkillStart && playerModel.currentState != PlayerState.BigSkill 
            && (playerController.inputSystem.Player.SwitchDown.triggered || playerController.inputSystem.Player.SwitchUp.triggered))
        {
            if (isContinuePlay == false)
            {
                //�л���ɫ
                playerController.SwitchNextModel(false);
            }
            else //�л���ɫ
                playerController.SwitchNextModel(true);
        }
        #endregion
    }

    //��������
    public bool IsAnimationEnd()
    {
        #region �������Ž���
        //ˢ�¶���״̬��Ϣ
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !playerModel.animator.IsInTransition(0));
        #endregion
    }

    public float NormalizedTime()
    {
        //ˢ�¶���״̬��Ϣ
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }

   


}
