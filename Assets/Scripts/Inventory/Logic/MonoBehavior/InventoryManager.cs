using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingleMonoBase<InventoryManager>
{

    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }


    [Header("Inventory Data")]
    public InventoryData_SO inventoryTemplate;

    public InventoryData_SO inventoryData;

    public InventoryData_SO actionTemplate;

    public InventoryData_SO actionData;

    [Header("ContainerS")]
    public ContainerUI inventoryUI;
    public ContainerUI actionUI;


    [Header("Drag Canvas")]
    public Canvas dragCanvas;
    public DragData currentDrag;

    [Header("UI Panel")]
    public GameObject bagPanel;

    [Header("Tooltip")]
    public ItemTooltip tooltip;

    bool isOpen = false;

    private protected override void Awake()
    {
        base.Awake();
        if (inventoryTemplate != null)
            inventoryData = Instantiate(inventoryTemplate);
        if (actionTemplate != null)
            actionData = Instantiate(actionTemplate);
    }


    private void Start()
    {
        LoadData();
        inventoryUI.RefreshUI();
        actionUI.RefreshUI();
    }

    private void Update()
    {
        if(PlayerController.INSTANCE.inputSystem.Player.Bag.triggered)
        {
            isOpen = !isOpen;
            bagPanel.SetActive(isOpen);
        }
    }

    public void SaveData()
    {
        SaveManager.INSTANCE.Save(inventoryData, inventoryData.name);
        SaveManager.INSTANCE.Save(actionData, actionData.name);
    }

    public  void LoadData()
    {
        SaveManager.INSTANCE.Load(inventoryData, inventoryData.name);
        SaveManager.INSTANCE.Load(actionData, actionData.name);
    }    


    #region 检查拖拽物品是否在每一个 Slot 范围内
    public bool CheckInInventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)inventoryUI.slotHolders[i].transform;
            if(RectTransformUtility.RectangleContainsScreenPoint(t,position))
            {
                return true;
            }    
        }
        return false;
    }
    public bool CheckInActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotHolders.Length; i++)
        {
            RectTransform t = (RectTransform)actionUI.slotHolders[i].transform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }
        return false;
    }

    #endregion

    #region 检测任务物品
    public void CheckQuestItemInBag(string questItemName)
    {
        foreach(var item in inventoryData.items)
        {
            if(item.itemData != null)
            {
                if(item.itemData.itemName == questItemName)
                {
                    QuestManager.INSTANCE.UpdateQuestProgress(item.itemData.itemName, item.amount);
                }
            }
        }

        foreach (var item in actionData.items)
        {
            if (item.itemData != null)
            {
                if (item.itemData.itemName == questItemName)
                {
                    QuestManager.INSTANCE.UpdateQuestProgress(item.itemData.itemName, item.amount);
                }
            }
        }

    }
    #endregion

    //检测背包和快捷栏物品
    public InventoryItem QuestItemInBag(ItemData_SO questItem)
    {
        return inventoryData.items.Find(i => i.itemData == questItem);
    }
    public InventoryItem QuestItemInAction(ItemData_SO questItem)
    {
        return actionData.items.Find(i => i.itemData == questItem);
    }
}
