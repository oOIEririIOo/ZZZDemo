using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class UnagiNormalAttackState : UnagiStateBase
{
    //是否进入下一段普通攻击
    private bool enterNextAttack;
    //是否锁定目标
    private bool isLock = false;

    private string modelName;

    private Camera mainCamera;


    public override void Enter()
    {

        base.Enter();
        mainCamera = Camera.main;
        enterNextAttack = false;
        isLock = false;
        isContinuePlay = true;

        LookToEnemy();

        //播放动画
        playerController.PlayAnimation("Attack_Normal_" + playerModel.characterStats.skillConfig.currentNormalAttackIndex);
        //Debug.Log(playerModel.gameObject.name);
        modelName = playerModel.gameObject.name;
        modelName = modelName.Replace("(Clone)", "");
        AudioManager.INSTANCE.PlayAudio(modelName + "攻击" + playerModel.characterStats.skillConfig.currentNormalAttackIndex);
        playerModel.characterStats.skillConfig.currentAttackInfo = playerModel.characterStats.skillConfig.normalAttack[playerModel.characterStats.skillConfig.currentNormalAttackIndex-1];
    }

    public override void Update()
    {
        base.Update();


        #region 攻击可转向
        if (NormalizedTime() <= 0.3f && isLock == false && playerController.inputMoveVec2 != Vector2.zero)
        {
            Vector3 inputMovec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
            //获取相机的旋转轴
            float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
            //四元数 * 向量
            Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMovec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);
            //计算旋转角度
            float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed * 50);
        }
        #endregion

        #region 时刻面对敌人
        if (isLock)
        {
            LookToEnemy();
        }
        #endregion

        #region 监听闪避
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

        #region 检测大招
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //进入大招状态
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

       

        #region 检测预输入

        if (!IsAnimationEnd() && playerController.inputSystem.Player.Fire.triggered && !enterNextAttack && !playerController.mouseOpen && NormalizedTime() >= 0.6f)
        {
            enterNextAttack = true;
        }

        #endregion

        #region 动画播放结束
        if (IsAnimationEnd())
        {
            #region 检测重击
            if (playerController.mousePressed)
            {
                playerController.SwitchState(PlayerState.Unagi_HavenAttack);
                return;
            }
            #endregion

            else if (enterNextAttack)
            {
                //切换到下一个攻击状态
                //累加攻击段数
                playerModel.characterStats.skillConfig.currentNormalAttackIndex++;
                if (playerModel.characterStats.skillConfig.currentNormalAttackIndex > playerModel.characterStats.skillConfig.normalAttack.Length)
                {
                    playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
                }
                //切换到普通攻击状态
                playerController.SwitchState(PlayerState.NormalAttack);
                return;
            }
            else
            {
                playerController.SwitchState(PlayerState.NormalAttackEnd);
            }

        }
        #endregion
    }



    private void LookToEnemy()
    {
        #region 锁定最近敌人
        GameObject targetEnemy = null;//目标敌人
        //初始化最近敌人的距离
        float minDistance = Mathf.Infinity;
        //遍历所有敌人标签
        foreach (string tag in playerController.enemyTagList)
        {
            //获取标签下的所有敌人
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {

                //计算敌人距离
                float distance = Vector3.Distance(playerModel.transform.position, enemy.transform.position);
                //比较
                if (distance < minDistance)
                {
                    targetEnemy = enemy;
                    minDistance = distance;
                }
            }
        }
        //如果距离够短再进行转向
        if (targetEnemy != null && minDistance <= 7f)
        {
            isLock = true;
            //计算玩家和最近敌人的方向
            Vector3 direction = (targetEnemy.transform.position - playerModel.transform.position).normalized;
            //玩家模型面朝敌人
            playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        #endregion
    }
}
