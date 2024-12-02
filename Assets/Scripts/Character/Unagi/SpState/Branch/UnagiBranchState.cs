using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiBranchState : UnagiStateBase
{
    public override void Enter()
    {
        base.Enter();
        playerController.branchTap = false;
        playerController.branchHold = false;
        isContinuePlay = true;
        //ÅÐ¶Ïsp
        switch(playerModel.currentState)
        {
            case PlayerState.Branch:
                playerModel.characterStats.skillConfig.currentBranchIndex = 1;
                playerController.PlayAnimation("Branch_1", 0f);
                break;
            case PlayerState.SpBranch:
                playerModel.characterStats.skillConfig.currentBranchIndex = 2;
                playerController.PlayAnimation("Branch_2", 0f);
                break;
            case PlayerState.Unagi_HoldBranch:
                playerModel.characterStats.skillConfig.currentBranchIndex = 3;
                playerController.PlayAnimation("Branch_3", 0f);
                break;
        }
        playerModel.characterStats.skillConfig.currentAttackInfo = playerModel.characterStats.skillConfig.branch[playerModel.characterStats.skillConfig.currentBranchIndex - 1];
        playerController.playerModel.characterStats.skillConfig.currentAttackInfo.hitIndex = -1;

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

        if (IsAnimationEnd())
        {
            #region ¼ì²âÖØ»÷
            if (playerController.mousePressed)
            {
                playerController.SwitchState(PlayerState.Unagi_HavenAttack);
                return;
            }
            #endregion
            switch (playerModel.currentState)
            {
                case PlayerState.Branch:
                    playerController.SwitchState(PlayerState.Branch_End);
                    break;
                case PlayerState.SpBranch:
                    playerController.SwitchState(PlayerState.SpBranch_End);
                    break;
                case PlayerState.Unagi_HoldBranch:
                    playerController.SwitchState(PlayerState.Unagi_HoldBranchEnd);
                    break;
            }
        }
        
    }
}
