using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiStateBase : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        for (int i = 0; i < playerModel.weapons.Length; i++)
        {
            playerModel.weapons[i].StopHit();
        }
        playerModel.currentVFXIndex = 0;
    }
}
