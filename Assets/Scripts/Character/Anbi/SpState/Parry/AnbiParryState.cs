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
        //������Һ�������˵ķ���
        Vector3 direction = (PlayerController.INSTANCE.parryTarget.transform.position - playerModel.transform.position).normalized;
        //���ģ���泯����
        playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    }

    public override void Update()
    {
        if (NormalizedTime() < 0.25f)
        {
            //������Һ�������˵ķ���
            Vector3 direction = (PlayerController.INSTANCE.parryTarget.transform.position - playerModel.transform.position).normalized;
            //���ģ���泯����
            playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
        base.Update();
        #region ��⶯������
        if (IsAnimationEnd())
        {
            // �л�����״̬
            //��ǰ������������
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
