using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大招开始
/// </summary>
public class PlayerBigSkillStartState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //切换镜头
        CameraManager.INSTANCE.cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        CameraManager.INSTANCE.virtualCamera.SetActive(false);
        playerModel.bigSkillStartShot.SetActive(true);

        //播放动画
        playerController.PlayAnimation("BigSkill_Start");
    }

    public override void Update()
    {
        base.Update();

        #region 检测动画
        if(IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.BigSkill);
        }
        #endregion
    }
}
