using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType { BAG,ACTION}
public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;

    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount % 2 == 0)
        {
            UseItem();
            UseBuff();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem() != null)
        {
            if (itemUI.GetItem().itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                PlayerController.INSTANCE.playerModel.characterStats.ApplyHealth(itemUI.GetItem().useableData.healthPoint);

                itemUI.Bag.items[itemUI.Index].amount -= 1;

                //检测任务进度
                QuestManager.INSTANCE.UpdateQuestProgress(itemUI.GetItem().itemName, -1);
            }
        }
        
        UpdateItem();
    }

    public void UseBuff()
    {
        if(itemUI.GetItem()!=null)
        {
            if (itemUI.GetItem().itemType == ItemType.BUFF && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                foreach(var model in PlayerController.INSTANCE.characterInfo)
                {
                    itemUI.GetItem().buffData.BUFFOn(model.GetComponent<PlayerModel>().characterStats.skillConfig, model.GetComponent<PlayerModel>().characterStats);
                }
                
                BuffManager.INSTANCE.UpdateIcon(itemUI.GetItem());
            }
        }
        UpdateItem();
    }


    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.INSTANCE.inventoryData;
                break;
            case SlotType.ACTION:
                itemUI.Bag = InventoryManager.INSTANCE.actionData;
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetupItemUI(item.itemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemUI.GetItem())
        {
            InventoryManager.INSTANCE.tooltip.SetupTooltip(itemUI.GetItem());
            InventoryManager.INSTANCE.tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.INSTANCE.tooltip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InventoryManager.INSTANCE.tooltip.gameObject.SetActive(false);
    }
}
