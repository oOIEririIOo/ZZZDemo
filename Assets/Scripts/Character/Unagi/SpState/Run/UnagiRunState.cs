using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiRunState : UnagiStateBase
{
    private Camera mainCamera;


    public override void Enter()
    {
        base.Enter();
        mainCamera = Camera.main;

        switch(playerModel.currentState)
        {
            case PlayerState.Walk:
                #region 左右脚判断 
                switch (playerModel.foot)
                {
                    case ModelFoot.Left:
                        playerController.PlayAnimation("Walk", 0.1f, 0.6f);
                        playerModel.foot = ModelFoot.Right;
                        break;
                    case ModelFoot.Right:
                        playerController.PlayAnimation("Walk", 0.1f, 0f);
                        playerModel.foot = ModelFoot.Left;
                        break;
                }
                #endregion
                break;
            case PlayerState.Run:
                playerController.PlayAnimation("Run",0.1f);
                break;
        }
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

        #region 检测技能
        if (playerController.inputSystem.Player.Branch.triggered)
        {
            playerController.SwitchState(PlayerState.Unagi_BranchStart);
            return;
        }
        #endregion

        #region 检测攻击
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            if(playerModel.currentState == PlayerState.Walk)
            {
                //切换到普通攻击状态
                playerController.SwitchState(PlayerState.NormalAttack);
                return;
            }
            else if(playerModel.currentState == PlayerState.Run)
            {
                playerController.SwitchState(PlayerState.Attack_Rush);
                return;
            }
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

        #region 检测待机
        if (playerController.inputMoveVec2 == Vector2.zero)
        {
            playerController.SwitchState(PlayerState.RunEnd);
            return;
        }
        #endregion
        else
        {
            #region 处理移动方向
            Vector3 inputMovec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
            //获取相机的旋转轴
            float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
            //四元数 * 向量
            Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMovec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);
            //计算旋转角度
            float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
            if(angles > 177.5 && angles <182.5 && playerModel.currentState == PlayerState.Run)
            {
                //切换到转身状态
                playerController.SwitchState(PlayerState.TurnBack);
            }
            else playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed);
            #endregion
        }
        


    }
}
