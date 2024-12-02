using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ͨ������ҡ
public class AnbiPerfectAttackEndState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        //������ͨ������ҡ
        playerController.PlayAnimation("Attack_Normal_4_Perfect_End");
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

        #region ��⹥��״̬
        if (playerController.inputSystem.Player.Fire.triggered && !playerController.mouseOpen)
        {
            //�ۼӹ�������
            playerModel.characterStats.skillConfig.currentNormalAttackIndex++;
            if (playerModel.characterStats.skillConfig.currentNormalAttackIndex > playerModel.skillConfig.normalAttack.Length)
            {
                playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            }
            //�л�����ͨ����״̬
            playerController.SwitchState(PlayerState.NormalAttack);
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

        #region �������
        #region ��������
        if (playerController.inputMoveVec2 != Vector2.zero && playerController.inputSystem.Player.Evade.triggered)
        {
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            playerController.SwitchState(PlayerState.Evade_Front);

            return;
        }
        else if (playerController.inputSystem.Player.Evade.triggered)
        {
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            playerController.SwitchState(PlayerState.Evade_Back);

            return;
        }
        #endregion
        #endregion

        #region �ƶ�����
        if (playerController.inputMoveVec2 != Vector2.zero && animationPlayTime > 0.5f)
        {
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            playerController.SwitchState(PlayerState.Walk);

            return;
        }
        #endregion
        /*
        if(NormalizedTime()>0.5f)
        {
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
        }
        */
        #region ��⶯������
        if (IsAnimationEnd())
        {
            // �л�����״̬
            //��ǰ������������
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            playerController.SwitchState(PlayerState.Idle);
            return;
            #endregion
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (playerModel.TryGetComponent<AnbiState>(out AnbiState anbiState))
        {
            anbiState.perfectTiming = false;
        }
    }

}