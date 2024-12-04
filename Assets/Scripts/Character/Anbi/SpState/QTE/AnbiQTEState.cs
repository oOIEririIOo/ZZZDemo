using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiQTEState : AnbiStateBase
{
    public override void Enter()
    {
        base.Enter();
        CameraHitFeel.INSTANCE.SwitichCharacterInQTE();
        isContinuePlay = true;
        playerController.playerModel.isQTE = true;
        //playerController.playerModel.characterController.enabled = false;
        if (PlayerController.INSTANCE.QTETarget != null)
        {
            //������Һ�������˵ķ���
            Vector3 direction = (PlayerController.INSTANCE.QTETarget.transform.position - playerModel.transform.position).normalized;
            //���ģ���泯����
            playerModel.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            //PlayerController.INSTANCE.QTETarget = null;
        }
        
        
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
        playerController.playerModel.isQTE = false;
        //playerController.playerModel.characterController.enabled = true;
    }

}
