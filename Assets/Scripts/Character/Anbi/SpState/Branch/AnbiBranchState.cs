using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiBranchState : AnbiStateBase
{
    bool isLock;
    public override void Enter()
    {
        base.Enter();
        isContinuePlay = true;
        //�ж�sp
        if (playerModel.characterStats.CurrentSP >= playerModel.characterStats.skillConfig.branch[2].SP)
        {
            playerModel.currentState = PlayerState.SpBranch;
            playerModel.characterStats.ApplySP(playerModel.characterStats.skillConfig.branch[2].SP);
            playerModel.characterStats.skillConfig.currentBranchIndex = 3;
            playerController.PlayAnimation("Branch_2", 0f);
        }
        else
        {
            playerModel.characterStats.skillConfig.currentBranchIndex = 1;
            playerModel.currentState = PlayerState.Branch;
            playerController.PlayAnimation("Branch_1", 0f);
        }
        playerModel.characterStats.skillConfig.currentAttackInfo = playerModel.characterStats.skillConfig.branch[playerModel.characterStats.skillConfig.currentBranchIndex - 1];
        playerController.playerModel.characterStats.skillConfig.currentAttackInfo.hitIndex = -1;
        LookToEnemy();

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

        if (IsAnimationEnd())
        {
            switch (playerModel.currentState)
            {
                case PlayerState.Branch:
                    playerController.SwitchState(PlayerState.Branch_End);
                    break;
                case PlayerState.SpBranch:
                    playerController.SwitchState(PlayerState.SpBranch_End);
                    break;
                case PlayerState.Anbi_PerfectBranch:
                    playerController.SwitchState(PlayerState.Anbi_PerfectBranch_End);
                    break;
                case PlayerState.Anbi_PerfectSPBranch:
                    playerController.SwitchState(PlayerState.Anbi_PerfectSPBranch_End);
                    break;
            }
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
    private void LookToEnemy()
    {
        #region �����������
        GameObject targetEnemy = null;//Ŀ�����
        //��ʼ��������˵ľ���
        float minDistance = Mathf.Infinity;
        //�������е��˱�ǩ
        foreach (string tag in playerController.enemyTagList)
        {
            //��ȡ��ǩ�µ����е���
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {

                //������˾���
                float distance = Vector3.Distance(playerModel.transform.position, enemy.transform.position);
                //�Ƚ�
                if (distance < minDistance)
                {
                    targetEnemy = enemy;
                    minDistance = distance;
                }
            }
        }
        //������빻���ٽ���ת��
        if (targetEnemy != null && minDistance <= 7f)
        {
            isLock = true;
            //������Һ�������˵ķ���
            Vector3 direction = (targetEnemy.transform.position - playerModel.transform.position).normalized;
            //���ģ���泯����
            playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        #endregion
    }
}