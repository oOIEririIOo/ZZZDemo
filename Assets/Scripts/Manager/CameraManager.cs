using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机管理器
/// </summary>
public class CameraManager : SingleMonoBase<CameraManager>
{
    //CM大脑组件
    public CinemachineBrain cinemachineBrain;
    //自由相机
    public GameObject virtualCamera;
    //弹反相机左
    public GameObject parryCameraLeft;
    //弹反相机右
    public GameObject parryCameraRight;
    //QTE相机
    public GameObject QTECamera;

    //自由相机组件
    public CinemachineVirtualCamera virtualCameraComponent;

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    
    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.P))
        {

            virtualCameraComponent.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 1f;

        }
        */
    }
    
    //启用弹反相机
    public void SwitichParryCamera()
    {
        cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 0.6f);
        //if(virtualCamera.transform)
        if(Vector3.Cross(PlayerController.INSTANCE.playerModel.transform.forward, virtualCamera.transform.position - PlayerController.INSTANCE.playerModel.transform.position).y <= 0)
        {
            parryCameraLeft.gameObject.SetActive(true);
        }
        else parryCameraRight.gameObject.SetActive(true);
    }

    //启用QTE相机
    public void OpenQTECamera()
    {
        cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 0.1f);
        QTECamera.transform.position = PlayerController.INSTANCE.playerModel.QTECameraPoint.position;
        QTECamera.transform.forward = PlayerController.INSTANCE.playerModel.QTECameraPoint.forward;
        QTECamera.gameObject.SetActive(true);
    }

    //重置自由相机视角
    public void ResetFreeLookCamera()
    {
        cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1f);
        parryCameraLeft.gameObject.SetActive(false);
        parryCameraRight.gameObject.SetActive(false);
        QTECamera.gameObject.SetActive(false);

    }


}
