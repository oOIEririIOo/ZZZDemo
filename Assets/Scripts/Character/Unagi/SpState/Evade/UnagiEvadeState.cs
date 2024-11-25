using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������״̬
/// </summary>
public class UnagiEvadeState : UnagiStateBase
{
    private Camera mainCamera;
    private bool isPreInput;
    public override void Enter()
    {
        mainCamera = Camera.main;
        base.Enter();
        playerController.isDodge = true;
        playerController.characterInfo[playerController.currentModelIndex].GetComponent<PlayerModel>().dodgeColl.enabled = true;
        isPreInput = false;
        playerController.evadeTimer = 0f;
        switch (playerModel.currentState)
        {
            case PlayerState.Evade_Front:
                playerController.PlayAnimation("Evade_Front",0.1f);
                Vector3 inputMovec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
                //��ȡ�������ת��
                float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
                //��Ԫ�� * ����
                Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMovec3;
                Quaternion targetQua = Quaternion.LookRotation(targetDic);
                //������ת�Ƕ�
                float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
                if(playerController.inputMoveVec2 != Vector2.zero)
                    playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed * 50);
                break;
            case PlayerState.Evade_Back:
                playerController.PlayAnimation("Evade_Back",0.1f);
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

        #region ��������
        if (playerController.inputSystem.Player.Evade.triggered && NormalizedTime() <= 0.833f)
        {
            isPreInput = true;
        }
        if (NormalizedTime() >= 0.5f && isPreInput && playerController.evadeCnt != 2)
        {
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            switch(playerModel.currentState)
            {
                case PlayerState.Evade_Front:
                    playerController.SwitchState(PlayerState.Evade_Front);
                    break;
                case PlayerState.Evade_Back:
                    playerController.SwitchState(PlayerState.Evade_Back);
                    break;
            }
                return;
        }
        if (playerController.inputMoveVec2 != Vector2.zero && playerController.inputSystem.Player.Evade.triggered && NormalizedTime()>=0.833f && playerController.evadeCnt != 2)
        {
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            playerController.SwitchState(PlayerState.Evade_Front);

            return;
        }
        else if (playerController.inputSystem.Player.Evade.triggered && NormalizedTime() >= 0.833f && playerController.evadeCnt != 2)
        {
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            playerController.SwitchState(PlayerState.Evade_Back);

            return;
        }
        #endregion

        #region ��⹥��
        if (playerController.inputSystem.Player.Fire.triggered && !playerController.mouseOpen)
        {
            playerController.SwitchState(PlayerState.Attack_Rush);
            return;
        }

        #endregion

        #region �Ƿ񲥷Ž���
        if (IsAnimationEnd())
            switch(playerModel.currentState)
            {
                case PlayerState.Evade_Front:
                    if(playerController.inputSystem.Player.Evade.IsPressed())
                    {
                        playerController.SwitchState(PlayerState.Run);
                        return;
                    }
                    playerController.SwitchState(PlayerState.Evade_Front_End);
                    break;
                case PlayerState.Evade_Back:
                    playerController.SwitchState(PlayerState.Evade_Back_End);
                    break;
            }
      
        #endregion
    }

    public override void Exit()
    {
        base.Exit();
        playerController.isDodge = false;
        playerController.characterInfo[playerController.currentModelIndex].GetComponent<PlayerModel>().dodgeColl.enabled = false;
    }
}
