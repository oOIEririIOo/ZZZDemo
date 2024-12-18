using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 180��ת��״̬
/// </summary>
public class UnagiTurnBackState : UnagiStateBase
{
    private Camera mainCamera;
    public override void Enter()
    {
        base.Enter();
        
        mainCamera = Camera.main;
        playerController.PlayAnimation("TurnBack", 0.15f);

        
    }

    public override void Update()
    {
        base.Update();

        if(NormalizedTime()>=0.1f)
        {
            #region �����ƶ�����
            Vector3 inputMovec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
            //��ȡ�������ת��
            float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
            //��Ԫ�� * ����
            Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMovec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);
            //������ת�Ƕ�
            float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed * 0.5f);
            #endregion
        }
        #region ������
        if (playerController.inputMoveVec2 == Vector2.zero)
        {
            playerController.SwitchState(PlayerState.RunEnd);
            return;
        }
        #endregion



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

    public override void Exit()
    {
        base.Exit();
        //playerModel.transform.Rotate(new Vector3(0, 180, 0));
    }

    
}
