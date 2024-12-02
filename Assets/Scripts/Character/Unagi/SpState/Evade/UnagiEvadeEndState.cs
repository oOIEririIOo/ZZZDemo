using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// …¡±‹∫Û“°
/// </summary>
public class UnagiEvadeEndState : UnagiStateBase
{
    public override void Enter()
    {
        base.Enter();

        #region ≈–∂œ…¡±‹¿‡–Õ
        switch(playerModel.currentState)
        {
            case PlayerState.Evade_Front_End:
                playerController.PlayAnimation("Evade_Front_End",0.1f);
                break;
            case PlayerState.Evade_Back_End:
                playerController.PlayAnimation("Evade_Back_End",0.1f);
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

        #region ºÏ≤‚ººƒ‹
        if (playerController.inputSystem.Player.Branch.triggered)
        {
            playerController.SwitchState(PlayerState.Branch);
            return;
        }
        #endregion

        #region ºÏ≤‚π•ª˜
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            if(playerController.perfectDodge)
            {
                playerController.SwitchState(PlayerState.Counter);
                playerController.perfectDodge = false;
                return;
            }
            else
            {
                //«–ªªµΩ∆’Õ®π•ª˜◊¥Ã¨
                playerController.SwitchState(PlayerState.Attack_Rush);
                return;
            }
            
        }
        #endregion

        #region “∆∂Øº‡Ã˝
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region º‡Ã˝…¡±‹

        if (playerController.inputSystem.Player.Evade.triggered)
        {
            switch (playerModel.currentState)
            { 
                case PlayerState.Evade_Front_End:
                    playerController.SwitchState(PlayerState.Evade_Front);
                    break;
                case PlayerState.Evade_Back_End:
                    playerController.SwitchState(PlayerState.Evade_Back);
                    break;
            }
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

    public override void Exit()
    {
        base.Exit();
        playerController.perfectDodge = false;
    }
}
