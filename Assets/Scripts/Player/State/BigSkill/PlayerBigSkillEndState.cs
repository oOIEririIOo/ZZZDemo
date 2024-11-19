using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigSkillEndState :PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //�л����ɾ�ͷ
        CameraManager.INSTANCE.cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1f);
        playerModel.bigSkillShot.SetActive(false);
        CameraManager.INSTANCE.virtualCamera.SetActive(true);
        CameraManager.INSTANCE.ResetFreeLookCamera();

        //���Ŷ���
        playerController.PlayAnimation("BigSkill_End",0f);
    }

    public override void Update()
    {
        base.Update();

        #region ��⹥��
        if (playerController.inputSystem.Player.Fire.triggered)
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

        #region ��⶯��
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
