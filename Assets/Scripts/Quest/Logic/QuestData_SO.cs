using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Quest",menuName = "Quest/Quest Data")]
public class QuestData_SO : ScriptableObject
{
    [System.Serializable]
    public class QuestRequire
    {
        public string name;
        public int requireAmount;
        public int currentAmount;
    }


    public string questName;
    [TextArea]
    public string description;

    public bool isStarted;
    public bool isComplete;
    public bool isFinished;

    public List<QuestRequire> questRequires = new List<QuestRequire>();
    public List<InventoryItem> rewards = new List<InventoryItem>();

    public void CheckQuestProgress()
    {
        var finishRequires = questRequires.Where(r => r.requireAmount <= r.currentAmount);
        isComplete = finishRequires.Count() == questRequires.Count;

        if(isComplete)
        {
            Debug.Log("任务完成！");
        }
    }

    public void GiveRewards()
    {
        foreach(var reward in rewards)
        {
            if(reward.amount <0)
            {
                int requireCount = Mathf.Abs(reward.amount);
                if(InventoryManager.INSTANCE.QuestItemInBag(reward.itemData) != null)
                {
                    if(InventoryManager.INSTANCE.QuestItemInBag(reward.itemData).amount <= requireCount)
                    {
                        requireCount -= InventoryManager.INSTANCE.QuestItemInBag(reward.itemData).amount;
                        InventoryManager.INSTANCE.QuestItemInBag(reward.itemData).amount = 0;

                        if(InventoryManager.INSTANCE.QuestItemInAction(reward.itemData) != null)
                        {
                            InventoryManager.INSTANCE.QuestItemInAction(reward.itemData).amount -= requireCount;
                        }
                    }
                    else
                    {
                        InventoryManager.INSTANCE.QuestItemInBag(reward.itemData).amount -= requireCount;
                    }
                }
                else
                {
                    InventoryManager.INSTANCE.QuestItemInAction(reward.itemData).amount -= requireCount;
                }
            }
            else
            {
                InventoryManager.INSTANCE.inventoryData.AddItem(reward.itemData, reward.amount);
            }
            InventoryManager.INSTANCE.inventoryUI.RefreshUI();
            InventoryManager.INSTANCE.actionUI.RefreshUI();
        }
    }


    //当前任务目标名字列表
    public List<string> RequireTargetName()
    {
        List<string> targetNameList = new List<string>();
        foreach (var require in questRequires)
        {
            targetNameList.Add(require.name);
        }
        return targetNameList;
    }
}
