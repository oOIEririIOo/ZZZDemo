using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// …¡±‹∫Û“°
/// </summary>
public class PlayerEvadeEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        #region ≈–∂œ…¡±‹¿‡–Õ
        switch(playerModel.currentState)
        {
            case PlayerState.Evade_Front_End:
                playerController.PlayAnimation("Evade_Front_End");
                break;
            case PlayerState.Evade_Back_End:
                playerController.PlayAnimation("Evade_Back_End");
                break;
        }
        #endregion
    }

    public override void Update()
    {
        base.Update();

        #region ºÏ≤‚¥Û’–
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //Ω¯»Î¥Û’–◊¥Ã¨
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        #region ºÏ≤‚π•ª˜
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //«–ªªµΩ∆’Õ®π•ª˜◊¥Ã¨
            playerController.SwitchState(PlayerState.Attack_Rush);
            return;
        }
        #endregion

        #region “∆∂Øº‡Ã˝
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region ∂Øª≠≤•∑≈Ω· ¯
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
        }
        #endregion
    }
}
