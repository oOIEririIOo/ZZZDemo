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
    //���������
    public GameObject parryCameraLeft;
    //���������
    public GameObject parryCameraRight;
    //QTE���
    public GameObject QTECamera;

    //����������
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
    
    //���õ������
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

    //����QTE���
    public void OpenQTECamera()
    {
        cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 0.1f);
        QTECamera.transform.position = PlayerController.INSTANCE.playerModel.QTECameraPoint.position;
        QTECamera.transform.forward = PlayerController.INSTANCE.playerModel.QTECameraPoint.forward;
        QTECamera.gameObject.SetActive(true);
    }

    //������������ӽ�
    public void ResetFreeLookCamera()
    {
        cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1f);
        parryCameraLeft.gameObject.SetActive(false);
        parryCameraRight.gameObject.SetActive(false);
        QTECamera.gameObject.SetActive(false);

    }


}
