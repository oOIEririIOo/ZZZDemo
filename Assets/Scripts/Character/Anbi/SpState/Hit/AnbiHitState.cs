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
            #region ������
            if (playerController.inputSystem.Player.BigSkill.triggered)
            {
                //�������״̬
                playerController.SwitchState(PlayerState.BigSkillStart);
                return;
            }
            #endregion

            #region ��⼼��
            if (playerController.inputSystem.Player.Branch.triggered)
            {
                playerController.SwitchState(PlayerState.Branch);
                return;
            }
            #endregion

            #region ��⹥��
            if (playerController.inputSystem.Player.Fire.triggered && !playerController.mouseOpen)
            {
                //�л�����ͨ����״̬
                playerController.SwitchState(PlayerState.NormalAttack);
                return;
            }

            #endregion

            #region �������
            if (playerController.inputSystem.Player.Evade.triggered)
            {
                //�л�������״̬
                playerController.SwitchState(PlayerState.Evade_Back);
                return;
            }

            #endregion

            #region ��������
            if (playerController.inputMoveVec2 != Vector2.zero)
            {
                //�л�������״̬
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
