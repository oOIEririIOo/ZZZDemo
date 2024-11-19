using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiBranchEndState : UnagiStateBase
{
    public override void Enter()
    {
        base.Enter();
        //�ж�sp
        switch (playerModel.currentState)
        {
            case PlayerState.Branch_End:
                playerController.PlayAnimation("Branch_1_End", 0f);
                break;
            case PlayerState.SpBranch_End:
                playerController.PlayAnimation("Branch_2_End", 0f);
                break;
            case PlayerState.Unagi_HoldBranchEnd:
                playerController.PlayAnimation("Branch_3_End", 0f);
                break;
        }
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
            playerController.SwitchState(PlayerState.Unagi_BranchStart);
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
        #region ��������
        if (playerController.inputMoveVec2 != Vector2.zero && playerController.inputSystem.Player.Evade.triggered)
        {
            playerController.SwitchState(PlayerState.Evade_Front);

            return;
        }
        else if (playerController.inputSystem.Player.Evade.triggered)
        {
            playerController.SwitchState(PlayerState.Evade_Back);

            return;
        }
        #endregion
        #endregion

        #region �ƶ�����
        if (playerController.inputMoveVec2 != Vector2.zero && animationPlayTime > 0.5f)
        {
            playerController.SwitchState(PlayerState.Walk);

            return;
        }
        #endregion
        #region ��⶯������
        if (IsAnimationEnd())
        {
            // �л�����״̬
            playerController.SwitchState(PlayerState.Idle);

            return;
            #endregion
        }

    }
}
