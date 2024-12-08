using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiBranchState : UnagiStateBase
{
    //�Ƿ�����Ŀ��
    private bool isLock = false;

    private Camera mainCamera;
    public override void Enter()
    {
        base.Enter();
        playerController.branchTap = false;
        playerController.branchHold = false;
        isContinuePlay = true;
        //�ж�sp
        switch(playerModel.currentState)
        {
            case PlayerState.Branch:
                playerModel.characterStats.skillConfig.currentBranchIndex = 1;
                playerController.PlayAnimation("Branch_1", 0f);
                break;
            case PlayerState.SpBranch:
                playerModel.characterStats.skillConfig.currentBranchIndex = 2;
                playerController.PlayAnimation("Branch_2", 0f);
                break;
            case PlayerState.Unagi_HoldBranch:
                playerModel.characterStats.skillConfig.currentBranchIndex = 3;
                playerController.PlayAnimation("Branch_3", 0f);
                break;
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
            #region ����ػ�
            if (playerController.mousePressed)
            {
                playerController.SwitchState(PlayerState.Unagi_HavenAttack);
                return;
            }
            #endregion
            switch (playerModel.currentState)
            {
                case PlayerState.Branch:
                    playerController.SwitchState(PlayerState.Branch_End);
                    break;
                case PlayerState.SpBranch:
                    playerController.SwitchState(PlayerState.SpBranch_End);
                    break;
                case PlayerState.Unagi_HoldBranch:
                    playerController.SwitchState(PlayerState.Unagi_HoldBranchEnd);
                    break;
            }
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
