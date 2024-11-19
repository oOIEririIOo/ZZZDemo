using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Text optionText;
    private Button thisButton;
    private DialoguePiece currentPiece;

    private bool takeQuest;

    private string nextPieceID;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece,DialogueOption option)
    {
        currentPiece = piece;
        optionText.text = option.text;
        nextPieceID = option.targetID;
        takeQuest = option.taskQuest;
    }

    public void OnOptionClicked()
    {
        if (currentPiece.quest != null)
        {
            var newTask = new QuestManager.QuestTask
            {
                questData = Instantiate(currentPiece.quest)
            };
            if(takeQuest)
            {
                //添加到任务列表
                //判断是否重复任务
                if(QuestManager.INSTANCE.HaveQuest(newTask.questData))
                {
                    //判断是否完成给予奖励
                    if(QuestManager.INSTANCE.GetTask(newTask.questData).IsComplete)
                    {
                        newTask.questData.GiveRewards();
                        QuestManager.INSTANCE.GetTask(newTask.questData).IsFinished = true;
                    }
                }
                else
                {
                    // 接受任务
                    QuestManager.INSTANCE.tasks.Add(newTask);
                    QuestManager.INSTANCE.GetTask(newTask.questData).IsStarted = true;

                    foreach (var requireItem in newTask.questData.RequireTargetName())
                    {
                        InventoryManager.INSTANCE.CheckQuestItemInBag(requireItem);
                    }
                }
            }
        }

        if (nextPieceID == "")
        {
            DialogueUI.INSTANCE.dialoguePanel.SetActive(false);
            return;
        }
        else
        {
            DialogueUI.INSTANCE.UpdataMainDialogue(DialogueUI.INSTANCE.currentData.dialogueIndex[nextPieceID]);
        }
    }
}
