using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiPerfectBranchState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        isContinuePlay = true;
        //�ж�sp
        if (playerModel.characterStats.CurrentSP >= playerModel.characterStats.skillConfig.branch[2].SP)
        {
            playerModel.currentState = PlayerState.Anbi_PerfectSPBranch;
            playerModel.characterStats.ApplySP(playerModel.characterStats.skillConfig.branch[2].SP);
            playerModel.characterStats.skillConfig.currentBranchIndex = 4;
            playerController.PlayAnimation("Branch_2_Perfect", 0f);
        }
        else
        {
            playerModel.currentState = PlayerState.Anbi_PerfectBranch;
            playerModel.characterStats.skillConfig.currentBranchIndex = 3;
            playerController.PlayAnimation("Branch_1_Perfect", 0f);
        }

        playerModel.characterStats.skillConfig.currentAttackInfo = playerModel.characterStats.skillConfig.branch[playerModel.characterStats.skillConfig.currentBranchIndex - 1];
        playerController.playerModel.characterStats.skillConfig.currentAttackInfo.hitIndex = -1;

    }

    public override void Update()
    {
        base.Update();

        #region ������
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //�������״̬
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