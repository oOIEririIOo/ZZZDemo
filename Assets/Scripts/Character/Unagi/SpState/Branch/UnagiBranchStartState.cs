using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiBranchStartState : UnagiStateBase
{
    public override void Enter()
    {
        base.Enter();
        //ÅÐ¶Ïsp
        playerController.PlayAnimation("Branch_1_Start", 0f);
    }

    public override void Update()
    {
        base.Update();
        if (!playerController.inputSystem.Player.Branch.IsPressed() && animationPlayTime > 0.3f)
        {
            if (playerModel.characterStats.CurrentSP >= playerModel.skillConfig.branch[1].SP)
            {
                //TODO:¼õÉÙSP
                playerModel.characterStats.ApplySP(playerModel.skillConfig.branch[1].SP);
                playerController.SwitchState(PlayerState.SpBranch);
                return;
            }
            else
            {
                playerController.SwitchState(PlayerState.Branch);
                return;
            }
                
        }

        #region ¼ì²â´óÕÐ
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //½øÈë´óÕÐ×´Ì¬
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        if (IsAnimationEnd())
        {
            if (playerModel.characterStats.CurrentSP >= playerModel.skillConfig.branch[2].SP)
            {
                playerModel.characterStats.ApplySP(playerModel.skillConfig.branch[2].SP);
                playerController.SwitchState(PlayerState.Unagi_HoldBranch);
                return;
            }
            else
            {
                playerController.SwitchState(PlayerState.Branch);
                return;
            }
        }
        
    }

}
