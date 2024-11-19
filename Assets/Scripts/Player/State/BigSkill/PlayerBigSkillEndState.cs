using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigSkillEndState :PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //ÇÐ»»×ÔÓÉ¾µÍ·
        CameraManager.INSTANCE.cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1f);
        playerModel.bigSkillShot.SetActive(false);
        CameraManager.INSTANCE.virtualCamera.SetActive(true);
        CameraManager.INSTANCE.ResetFreeLookCamera();

        //²¥·Å¶¯»­
        playerController.PlayAnimation("BigSkill_End",0f);
    }

    public override void Update()
    {
        base.Update();

        #region ¼ì²â¹¥»÷
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //ÇÐ»»µ½ÆÕÍ¨¹¥»÷×´Ì¬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }

        #endregion

        #region ¼ì²âÉÁ±Ü
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //ÇÐ»»µ½ÉÁ±Ü×´Ì¬
            playerController.SwitchState(PlayerState.Evade_Back);
            return;
        }

        #endregion

        #region ¼àÌý±¼ÅÜ
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            //ÇÐ»»µ½±¼ÅÜ×´Ì¬
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region ¼ì²â¶¯»­
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
