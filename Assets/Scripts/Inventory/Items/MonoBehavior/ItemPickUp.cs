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
            //�����Ʒ������
            InventoryManager.INSTANCE.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.INSTANCE.inventoryUI.RefreshUI();

            //����Ƿ�������
            QuestManager.INSTANCE.UpdateQuestProgress(itemData.itemName, itemData.itemAmount);

            Destroy(gameObject);


        }
    }
}
