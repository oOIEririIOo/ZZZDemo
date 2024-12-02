using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitichManager : SingleMonoBase<CameraSwitichManager>
{
    private CinemachineBrain brain;

    private protected override void Awake()
    {
        base.Awake();
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }
}
