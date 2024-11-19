using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiBranchStartState : UnagiStateBase
{
    public override void Enter()
    {
        base.Enter();
        //�ж�sp
        playerController.PlayAnimation("Branch_1_Start", 0f);
    }

    public override void Update()
    {
        base.Update();
        if (!playerController.inputSystem.Player.Branch.IsPressed() && animationPlayTime > 0.3f)
        {
            if (playerModel.characterStats.CurrentSP >= playerModel.skillConfig.branch[1].SP)
            {
                //TODO:����SP
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
