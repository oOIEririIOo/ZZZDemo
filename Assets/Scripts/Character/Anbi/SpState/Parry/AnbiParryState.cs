using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiParryState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        CameraManager.INSTANCE.SwitichParryCamera();
        playerController.isParry = true;
        playerController.PlayAnimation("Parry", 0.1f);
        //计算玩家和最近敌人的方向
        Vector3 direction = (PlayerController.INSTANCE.parryTarget.transform.position - playerModel.transform.position).normalized;
        //玩家模型面朝敌人
        playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    }

    public override void Update()
    {
        if (NormalizedTime() < 0.25f)
        {
            //计算玩家和最近敌人的方向
            Vector3 direction = (PlayerController.INSTANCE.parryTarget.transform.position - playerModel.transform.position).normalized;
            //玩家模型面朝敌人
            playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
        base.Update();
        #region 检测动画结束
        if (IsAnimationEnd())
        {
            // 切换待机状态
            //当前攻击段数归零
            playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
            playerController.SwitchState(PlayerState.ParryEnd);
            return;
            #endregion
        }
    }

    public override void Exit()
    {
        base.Exit();
        playerController.isParry = false;
        PlayerController.INSTANCE.parryTarget = null;
        CameraManager.INSTANCE.ResetFreeLookCamera();
    }
}
