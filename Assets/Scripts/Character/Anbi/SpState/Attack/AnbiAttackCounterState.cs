using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiAttackCounterState : AnbiStateBase
{

    //�Ƿ�����Ŀ��
    private bool isLock = false;

    //�Ƿ������һ����ͨ����
    private bool enterNextAttack;
    
    private Camera mainCamera;
    public override void Enter()
    {
        base.Enter();
        mainCamera = Camera.main;
        enterNextAttack = false;
        isLock = false;
        playerController.PlayAnimation("Attack_Counter");
    }

    public override void Update()
    {
        base.Update();

        #region ������ת��
        if (NormalizedTime() <= 0.3f && isLock == false && playerController.inputMoveVec2 != Vector2.zero)
        {
            Vector3 inputMovec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
            //��ȡ�������ת��
            float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
            //��Ԫ�� * ����
            Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMovec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);
            //������ת�Ƕ�
            float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed * 50);
        }
        #endregion

        #region ʱ����Ե���
        if (isLock)
        {
            LookToEnemy();
        }
        #endregion

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
            if (enterNextAttack)
            {
                //�л�����һ������״̬
                //�ۼӹ�������
                if (playerModel.characterStats.skillConfig.currentNormalAttackIndex > playerModel.characterStats.skillConfig.normalAttack.Length)
                {
                    playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
                }
                //�л�����ͨ����״̬
                playerController.SwitchState(PlayerState.NormalAttack);
                return;
            }
            else
            {
                playerController.SwitchState(PlayerState.Attack_Rush_End);
            }

        }
        #endregion
    }

    public override void Exit()
    {
        base.Exit();
        //��ǰ������������
        if (enterNextAttack)
        {
            //�л�����һ������״̬
            //�ۼӹ�������
            if (playerModel.characterStats.skillConfig.currentNormalAttackIndex > playerModel.characterStats.skillConfig.normalAttack.Length)
            {
                playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
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