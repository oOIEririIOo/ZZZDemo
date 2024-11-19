using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¥Û’–
/// </summary>
public class PlayerBigSkillState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        //«–ªªæµÕ∑
        playerModel.bigSkillStartShot.SetActive(false);
        playerModel.bigSkillShot.SetActive(true);

        //≤•∑≈∂Øª≠
        playerController.PlayAnimation("BigSkill",0f);
    }

    public override void Update()
    {
        base.Update();

        #region ºÏ≤‚∂Øª≠
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.BigSkillEnd);
        }
        #endregion
    }
}
