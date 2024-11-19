using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnBackEndState : PlayerStateBase
{
    private Camera mainCamera;
    public override void Enter()
    {
        base.Enter();
        mainCamera = Camera.main;
        playerController.PlayAnimation("TurnBack_End", 0.15f);


    }

    public override void Update()
    {
        base.Update();
        #region 处理移动方向
        Vector3 inputMovec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
        //获取相机的旋转轴
        float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
        //四元数 * 向量
        Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMovec3;
        Quaternion targetQua = Quaternion.LookRotation(targetDic);
        //计算旋转角度
        float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
        playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed);
        #endregion



        #region 检测大招
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //进入大招状态
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        #region 检测攻击
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //切换到普通攻击状态
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region 检测闪避
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //切换到闪避状态
            playerController.SwitchState(PlayerState.Evade_Front);
            return;
        }
        #endregion

        #region 是否播放结束
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Run);
        }
        #endregion
    }
}
