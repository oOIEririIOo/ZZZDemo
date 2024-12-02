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

    //����������
    public CinemachineVirtualCamera virtualCameraComponent;

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    /*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            
                var x =( Vector3.Cross(PlayerController.INSTANCE.playerModel.transform.forward, virtualCamera.transform.position - PlayerController.INSTANCE.playerModel.transform.position).y);

        }
    }
    */
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

    //������������ӽ�
    public void ResetFreeLookCamera()
    {
        cinemachineBrain.m_DefaultBlend = new Cinemachine.CinemachineBlendDefinition(Cinemachine.CinemachineBlendDefinition.Style.EaseInOut, 1.5f);
        parryCameraLeft.gameObject.SetActive(false);
        parryCameraRight.gameObject.SetActive(false);

    }
}
