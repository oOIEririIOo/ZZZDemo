using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //添加物品到背包
            InventoryManager.INSTANCE.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.INSTANCE.inventoryUI.RefreshUI();

            //检查是否有任务
            QuestManager.INSTANCE.UpdateQuestProgress(itemData.itemName, itemData.itemAmount);

            Destroy(gameObject);


        }
    }
}
