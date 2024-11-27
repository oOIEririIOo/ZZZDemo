using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiCounterState : UnagiStateBase
{
    
    //�Ƿ�����Ŀ��
    private bool isLock = false;
    //�Ƿ������һ����ͨ����
    private bool enterNextAttack;

    public override void Enter()
    {
        base.Enter();
        playerController.PlayAnimation("Attack_Counter", 0.1f);
        isLock = false;
        isContinuePlay = true;
        enterNextAttack = false;
        LookToEnemy();
        int index = 5;
        playerModel.characterStats.skillConfig.currentAttackInfo = playerModel.characterStats.skillConfig.branch[index -1];
        playerController.playerModel.characterStats.skillConfig.currentAttackInfo.hitIndex = -1;
    }

    public override void Update()
    {
        base.Update();
        /*
        #region ʱ����Ե���
        if (isLock)
        {
            LookToEnemy();
        }
        #endregion
        */

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

        #region ������
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //�������״̬
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        #region ���Ԥ����

        if (!IsAnimationEnd() && playerController.inputSystem.Player.Fire.triggered && !enterNextAttack && !playerController.mouseOpen && NormalizedTime() >= 0.6f)
        {
            enterNextAttack = true;
        }
        #endregion

        #region �������Ž���
        if (IsAnimationEnd())
        {
            #region ����ػ�
            if (playerController.mousePressed)
            {
                playerController.SwitchState(PlayerState.Unagi_HavenAttack);
                return;
            }
            #endregion

            else if (enterNextAttack)
            {
                //�л�����һ������״̬
                //�ۼӹ�������
                playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
                //�л�����ͨ����״̬
                playerController.SwitchState(PlayerState.NormalAttack);
                return;
            }
            else
            {
                playerController.SwitchState(PlayerState.Counter_End);
            }

        }
        #endregion
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
