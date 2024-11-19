using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        switch(playerModel.currentState)
        {
            case PlayerState.Idle:
                playerController.PlayAnimation("Idle");
                break;
            case PlayerState.Idle_AFK:
                playerController.PlayAnimation("Idle_AFK");
                break;
        }
       
    }

    public override void Update()
    {
        base.Update();

        #region ¼ì²â´óÕÐ
        if(playerController.inputSystem.Player.BigSkill.triggered)
        {
            //½øÈë´óÕÐ×´Ì¬
            playerController.SwitchState(PlayerState.BigSkillStart);
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

        switch (playerModel.currentState)
        {
            case PlayerState.Idle:
                #region ¼ì²â¹Ò»ú
                if (animationPlayTime > 3f)
                {
                    //ÇÐ»»µ½´ý»ú×´Ì¬
                    playerController.SwitchState(PlayerState.Idle_AFK);
                }
                break;
            #endregion
            case PlayerState.Idle_AFK:
                #region ¼ì²â¹Ò»ú¶¯»­²¥·Å½áÊø
                if(IsAnimationEnd())
                {
                    playerController.SwitchState(PlayerState.Idle);
                }
                #endregion
                break;
        }
    }


}
