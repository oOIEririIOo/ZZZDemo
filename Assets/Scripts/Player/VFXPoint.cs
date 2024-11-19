using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPoint : MonoBehaviour
{
    private float Height;
    public int characterIndex;

    private void Awake()
    {
        Height = transform.position.y;
    }

    private void Update()
    {
        //Vector3 playerPos = PlayerController.INSTANCE.playerModel.transform.position;
        Vector3 playerPos = PlayerController.INSTANCE.controllableModels[characterIndex].transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y + Height, playerPos.z);
        //Vector3 playerForward = PlayerController.INSTANCE.playerModel.transform.forward;
        Vector3 playerForward = PlayerController.INSTANCE.controllableModels[characterIndex].transform.forward;
        transform.forward = new Vector3(playerForward.x, playerForward.y, playerForward.z);
    }
}
