using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class PlayerBigSkillState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
        //�л���ͷ
        playerModel.bigSkillStartShot.SetActive(false);
        playerModel.bigSkillShot.SetActive(true);

        //���Ŷ���
        playerController.PlayAnimation("BigSkill",0f);
    }

    public override void Update()
    {
        base.Update();

        #region ��⶯��
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.BigSkillEnd);
        }
        #endregion
    }
}
