using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ½ÇÉ«ÆÕÍ¨Èë³¡×´Ì¬
/// </summary>
public class PlayerSwitchInNoramlState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerController.PlayAnimation("SwitchIn_Normal", 0f);
    }

    public override void Update()
    {
        base.Update();

        #region ¼ì²â´óÕÐ
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //½øÈë´óÕÐ×´Ì¬
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

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

        #region ¼ì²â¶¯»­½áÊø
        if(IsAnimationEnd())
        {
            //ÇÐ»»µ½´ý»ú×´Ì¬
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
