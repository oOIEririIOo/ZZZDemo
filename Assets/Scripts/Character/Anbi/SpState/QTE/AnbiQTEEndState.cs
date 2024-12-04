using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiQTEEndState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        playerController.PlayAnimation("QTE_End", 0.1f);
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
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
    }
}
