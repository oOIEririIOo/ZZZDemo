using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���п�ʼ
/// </summary>
public class PlayerBigSkillStartState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //�л���ͷ
        CameraManager.INSTANCE.cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        CameraManager.INSTANCE.virtualCamera.SetActive(false);
        playerModel.bigSkillStartShot.SetActive(true);

        //���Ŷ���
        playerController.PlayAnimation("BigSkill_Start");
    }

    public override void Update()
    {
        base.Update();

        #region ��⶯��
        if(IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.BigSkill);
        }
        #endregion
    }
}
