using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiQTEState : UnagiStateBase
{
    public override void Enter()
    {
        base.Enter();
        
        
        useGravity = false;
        playerController.playerModel.isQTE = true;
        CameraHitFeel.INSTANCE.SwitichCharacterInQTE();
        if (PlayerController.INSTANCE.QTETarget != null)
        {
            //计算玩家和最近敌人的方向
            Vector3 direction = (PlayerController.INSTANCE.QTETarget.transform.position - playerModel.transform.position).normalized;
            //玩家模型面朝敌人
            playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            //PlayerController.INSTANCE.QTETarget = null;
        }
        if (playerModel.QTECameraPoint != null)
        {
            CameraManager.INSTANCE.OpenQTECamera();
        }
        playerController.playerModel.characterController.enabled = false;
        
        //playerController.playerModel.characterController.excludeLayers = playerController.everythingLayer;
        playerModel.characterStats.skillConfig.currentAttackInfo = playerModel.characterStats.skillConfig.branch[6-1];
        playerController.playerModel.characterStats.skillConfig.currentAttackInfo.hitIndex = -1;
        playerController.PlayAnimation("QTE", 0.1f);
    }

    public override void Update()
    {
        base.Update();
        if(IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.QTE_End);
            return;
        }
    }
    public override void Exit()
    {
        base.Exit();
        CameraManager.INSTANCE.ResetFreeLookCamera();
        playerController.playerModel.characterController.enabled = true;
        playerController.playerModel.isQTE = false;
        //useGravity = true;
        //playerController.playerModel.gravity = -9.8f;
        //playerController.playerModel.characterController.excludeLayers = playerController.nothingLayer;
    }

}
