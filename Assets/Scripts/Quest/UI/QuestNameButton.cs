using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNameButton : MonoBehaviour
{
    public Text questNameText;
    public QuestData_SO currentData;
    public Text questContenText;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    void UpdateQuestContent()
    {
        questContenText.text = currentData.description;
        QuestUI.INSTANCE.SetupRequireList(currentData);

        foreach(Transform item in QuestUI.INSTANCE.rewardTransform)
        {
            Destroy(item.gameObject);
        }

        foreach(var item in currentData.rewards)
        {
            QuestUI.INSTANCE.SetupRewardItem(item.itemData, item.amount);
        }
    }

    public void SetupNameButton(QuestData_SO questData)
    {
        currentData = questData;
        if(questData.isFinished)
        {
            questNameText.text = questData.questName + "(Íê³É)";
        }
        else
        {
            questNameText.text = questData.questName;
        }    
    }
}
