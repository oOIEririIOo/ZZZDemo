using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;
    bool canTalk = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DialogueUI.INSTANCE.dialoguePanel.SetActive(false);
            canTalk = false;
        }
    }

    private void Update()
    {
        if(PlayerController.INSTANCE.inputSystem.Player.Talk.triggered && canTalk)
        {
            OpenDialogue();
        }
    }

    void OpenDialogue()
    {
        //打开UI面板
        //传输对话信息
        DialogueUI.INSTANCE.UpdataDialogueData(currentData);
        DialogueUI.INSTANCE.UpdataMainDialogue(currentData.dialoguePieces[0]);
    }
}
