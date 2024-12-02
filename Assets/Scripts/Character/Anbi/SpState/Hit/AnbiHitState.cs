using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiHitState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        switch (playerModel.hitType)
        {
            case HitType.Light:
                switch (playerModel.damageTrans)
                {
                    case DamageDir.Front:
                        playerController.PlayAnimation("Hit_L_Front", 0f);
                        break;
                    case DamageDir.Back:
                        playerController.PlayAnimation("Hit_L_Back", 0f);
                        break;
                }
                break;
            case HitType.Haven:
                switch (playerModel.damageTrans)
                {
                    case DamageDir.Front:
                        playerController.PlayAnimation("Hit_H_Front", 0f);
                        break;
                    case DamageDir.Back:
                        playerController.PlayAnimation("Hit_H_Back", 0f);
                        break;
                }
                break;
            case HitType.Fly:
                switch (playerModel.damageTrans)
                {
                    case DamageDir.Front:
                        playerController.PlayAnimation("HitFly_Front", 0f);
                        break;
                    case DamageDir.Back:
                        playerController.PlayAnimation("HitFly_Back", 0f);
                        break;
                }
                break;
        }
        playerModel.animator.Update(0);
        playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
    }

    public override void Update()
    {
        base.Update();

        if (NormalizedTime() >= 0.6f)
        {
            #region ¼ì²â´óÕÐ
            if (playerController.inputSystem.Player.BigSkill.triggered)
            {
                //½øÈë´óÕÐ×´Ì¬
                playerController.SwitchState(PlayerState.BigSkillStart);
                return;
            }
            #endregion

            #region ¼ì²â¼¼ÄÜ
            if (playerController.inputSystem.Player.Branch.triggered)
            {
                playerController.SwitchState(PlayerState.Branch);
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
        }

        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
        }
    }


}
