using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnBackEndState : PlayerStateBase
{
    private Camera mainCamera;
    public override void Enter()
    {
        base.Enter();
        mainCamera = Camera.main;
        playerController.PlayAnimation("TurnBack_End", 0.15f);


    }

    public override void Update()
    {
        base.Update();
        #region �����ƶ�����
        Vector3 inputMovec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
        //��ȡ�������ת��
        float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
        //��Ԫ�� * ����
        Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMovec3;
        Quaternion targetQua = Quaternion.LookRotation(targetDic);
        //������ת�Ƕ�
        float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed);
        #endregion



        #region ������
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //�������״̬
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

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
            playerController.SwitchState(PlayerState.Evade_Front);
            return;
        }
        #endregion

        #region �Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Run);
        }
        #endregion
    }
}
