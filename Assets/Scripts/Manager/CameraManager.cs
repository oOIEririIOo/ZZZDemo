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
    //自由相机组件
    public CinemachineVirtualCamera virtualCameraComponent;

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //重置自由相机视角
    public void ResetFreeLookCamera()
    {
        //virtualCameraComponent.m_YAxis.Value = 0.5f;
        //freeLook.m_XAxis.Value = PlayerController.INSTANCE.playerModel.transform.eulerAngles.y;
        
    }
}
