using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiBranchState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        isContinuePlay = true;
        //ÅÐ¶Ïsp
        if (playerModel.characterStats.CurrentSP >= playerModel.characterStats.skillConfig.branch[2].SP)
        {
            playerModel.currentState = PlayerState.SpBranch;
            playerModel.characterStats.ApplySP(playerModel.characterStats.skillConfig.branch[2].SP);
            playerModel.characterStats.skillConfig.currentBranchIndex = 2;
            playerController.PlayAnimation("Branch_2", 0f);
        }
        else
        {
            playerModel.characterStats.skillConfig.currentBranchIndex = 1;
            playerModel.currentState = PlayerState.Branch;
            playerController.PlayAnimation("Branch_1", 0f);
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
            switch (playerModel.currentState)
            {
                case PlayerState.Branch:
                    playerController.SwitchState(PlayerState.Branch_End);
                    break;
                case PlayerState.SpBranch:
                    playerController.SwitchState(PlayerState.SpBranch_End);
                    break;
                case PlayerState.Anbi_PerfectBranch:
                    playerController.SwitchState(PlayerState.Anbi_PerfectBranch_End);
                    break;
                case PlayerState.Anbi_PerfectSPBranch:
                    playerController.SwitchState(PlayerState.Anbi_PerfectSPBranch_End);
                    break;
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
        if (playerModel.TryGetComponent<AnbiState>(out AnbiState anbiState))
        {
            anbiState.perfectTiming = false;
        }
    }
}