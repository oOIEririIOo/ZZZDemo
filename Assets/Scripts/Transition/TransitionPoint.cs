using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene, DiffScene
    }

    [Header("Transtion Info")]
    public string sceneName;
    public TransitionType transitionType;

    public TransitionDestination.DestinationTag destinationTag;

    private bool canTrans;

    // ‰»ÎœµÕ≥
    public InputSystem inputSystem;


    private void Awake()
    {
        inputSystem = new InputSystem();
    }
    private void Update()
    {
        if(inputSystem.Player.interaction.triggered && canTrans)
        {
            //TP
            SceneController.INSTANCE.TPtoDestination(this);
            canTrans = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            canTrans = true;
            Debug.Log("canTP");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = false;
        }
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
