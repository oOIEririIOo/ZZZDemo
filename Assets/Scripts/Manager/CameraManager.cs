using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������
/// </summary>
public class CameraManager : SingleMonoBase<CameraManager>
{
    //CM�������
    public CinemachineBrain cinemachineBrain;
    //�������
    public GameObject virtualCamera;
    //����������
    public CinemachineVirtualCamera virtualCameraComponent;

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    //������������ӽ�
    public void ResetFreeLookCamera()
    {
        //virtualCameraComponent.m_YAxis.Value = 0.5f;
        //freeLook.m_XAxis.Value = PlayerController.INSTANCE.playerModel.transform.eulerAngles.y;
        
    }
}
