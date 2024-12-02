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
    //玩家状态
    PlayerState state;
    //玩家控制器
    protected PlayerController playerController;
    //玩家模型
    protected PlayerModel playerModel;
    //动画信息
    private AnimatorStateInfo stateInfo;
    //记录当前状态进入的时间
    protected float animationPlayTime = 0f;
    //是否切人后继续播放动画
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
        //施加重力
        playerModel.characterController.Move(new Vector3(0, playerModel.gravity * Time.deltaTime, 0));

        if(playerController.mouseOpen)
        {
            playerController.SwitchState(PlayerState.Pause);
        }

        //状态进入计时
        animationPlayTime += Time.deltaTime;

        #region 检测角色切换
        if(playerModel.currentState != PlayerState.BigSkillStart && playerModel.currentState != PlayerState.BigSkill 
            && (playerController.inputSystem.Player.SwitchDown.triggered || playerController.inputSystem.Player.SwitchUp.triggered))
        {
            if (isContinuePlay == false)
            {
                //切换角色
                playerController.SwitchNextModel(false);
            }
            else //切换角色
                playerController.SwitchNextModel(true);
        }
        #endregion
    }

    //动画结束
    public bool IsAnimationEnd()
    {
        #region 动画播放结束
        //刷新动画状态信息
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);
        return (stateInfo.normalizedTime >= 1.0f && !playerModel.animator.IsInTransition(0));
        #endregion
    }

    public float NormalizedTime()
    {
        //刷新动画状态信息
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime;
    }

   


}
