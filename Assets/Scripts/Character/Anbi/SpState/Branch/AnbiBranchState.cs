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
        //判断sp
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

        #region 检测大招
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //进入大招状态
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