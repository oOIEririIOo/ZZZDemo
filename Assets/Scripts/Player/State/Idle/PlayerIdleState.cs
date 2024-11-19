using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        switch(playerModel.currentState)
        {
            case PlayerState.Idle:
                playerController.PlayAnimation("Idle");
                break;
            case PlayerState.Idle_AFK:
                playerController.PlayAnimation("Idle_AFK");
                break;
        }
       
    }

    public override void Update()
    {
        base.Update();

        #region ������
        if(playerController.inputSystem.Player.BigSkill.triggered)
        {
            //�������״̬
            playerController.SwitchState(PlayerState.BigSkillStart);
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

        switch (playerModel.currentState)
        {
            case PlayerState.Idle:
                #region ���һ�
                if (animationPlayTime > 3f)
                {
                    //�л�������״̬
                    playerController.SwitchState(PlayerState.Idle_AFK);
                }
                break;
            #endregion
            case PlayerState.Idle_AFK:
                #region ���һ��������Ž���
                if(IsAnimationEnd())
                {
                    playerController.SwitchState(PlayerState.Idle);
                }
                #endregion
                break;
        }
    }


}
