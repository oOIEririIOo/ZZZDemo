using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiParryEndState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        playerController.PlayAnimation("Parry_End", 0.1f);
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
            
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            //�л�����ͨ����״̬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;

        }
        #endregion

        #region ��⼼��
        if (playerController.inputSystem.Player.Branch.triggered)
        {
            if (playerModel.TryGetComponent<AnbiState>(out AnbiState anbiState) && anbiState.perfectTiming)
            {
                playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
                playerController.SwitchState(PlayerState.Anbi_PerfectBranch);
            }
            else
            {
                playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
                playerController.SwitchState(PlayerState.Branch);
            }

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
}
