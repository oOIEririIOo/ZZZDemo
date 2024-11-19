using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBranchState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        playerController.PlayAnimation("Branch_"+playerModel.skillConfig.currentBranchIndex);
    }

    public override void Update()
    {
        base.Update();

        if(IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
        }
    }
}
