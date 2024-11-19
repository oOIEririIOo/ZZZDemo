using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܺ�ҡ
/// </summary>
public class UnagiEvadeEndState : UnagiStateBase
{
    public override void Enter()
    {
        base.Enter();

        #region �ж���������
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
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //�л�����ͨ����״̬
            playerController.SwitchState(PlayerState.Attack_Rush);
            return;
        }
        #endregion

        #region �ƶ�����
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region ��������

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

        #region �������Ž���
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
        }
        #endregion
    }
}
