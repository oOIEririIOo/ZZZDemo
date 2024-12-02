using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiAttackCounterEndState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerController.PlayAnimation("Attack_Counter_End");
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

        #region ¼ì²â¼¼ÄÜ
        if (playerController.inputSystem.Player.Branch.triggered)
        {
            playerController.SwitchState(PlayerState.Branch);
            return;
        }
        #endregion

        #region ¼ì²â¹¥»÷
        if (playerController.inputSystem.Player.Fire.triggered && !playerController.mouseOpen)
        {
            //ÇÐ»»µ½ÆÕÍ¨¹¥»÷×´Ì¬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }

        #endregion

        #region ¼ì²âÉÁ±Ü
        #region ¼àÌýÉÁ±Ü
        if (playerController.inputMoveVec2 != Vector2.zero && playerController.inputSystem.Player.Evade.triggered)
        {
            playerController.SwitchState(PlayerState.Evade_Front);

            return;
        }
        else if (playerController.inputSystem.Player.Evade.triggered)
        {
            playerController.SwitchState(PlayerState.Evade_Back);

            return;
        }
        #endregion
        #endregion

        #region ÒÆ¶¯¼àÌý
        if (playerController.inputMoveVec2 != Vector2.zero && animationPlayTime > 0.5f)
        {
            playerController.SwitchState(PlayerState.Walk);

            return;
        }
        #endregion
        #region ¼ì²â¶¯»­½áÊø
        if (IsAnimationEnd())
        {
            // ÇÐ»»´ý»ú×´Ì¬
            playerController.SwitchState(PlayerState.Idle);

            return;
            #endregion
        }
    }
}
