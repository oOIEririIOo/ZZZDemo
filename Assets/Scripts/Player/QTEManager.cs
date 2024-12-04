using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : SingleMonoBase<QTEManager>
{
    public bool waitQTEInput;
    public bool canQTE;
    public int QTECount;
    private void Start()
    {
        waitQTEInput = false;
        canQTE = false;
        QTECount = 0;
    }
    private void Update()
    {
        if(waitQTEInput)
        {
            QTEInput();
        }
        
    }
    public void QTEInput()
    {
        if(PlayerController.INSTANCE.inputSystem.QTE.Switich.triggered)
        {
            QTECount++;    
            PlayerController.INSTANCE.SwitchNextModel(PlayerController.SwitichType.QTE);
            
        }
    }

    public void CancelQTE()
    {
        QTECount = 0;
        waitQTEInput = false;
        canQTE = false;
        PlayerController.INSTANCE.QTETarget.isStun = true;
        PlayerController.INSTANCE.QTETarget = null;
        CameraHitFeel.INSTANCE.CancelQTE();
    }
    public void QTEUI()
    {

    }
    public void DisablePlayerInput()
    {
        PlayerController.INSTANCE.inputSystem.Player.Disable();
    }
    public void StartPlayerInput()
    {
        PlayerController.INSTANCE.inputSystem.Player.Enable();
    }
}
