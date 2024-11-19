using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;


    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.INSTANCE.currentDrag = new InventoryManager.DragData();
        InventoryManager.INSTANCE.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.INSTANCE.currentDrag.originalParent = (RectTransform)transform.parent;

        //记录原始数据
        transform.SetParent(InventoryManager.INSTANCE.dragCanvas.transform, true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标位置
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品 交换数据
        // 是否指向UI物品
        if(EventSystem.current.IsPointerOverGameObject())
        {
            if(InventoryManager.INSTANCE.CheckInActionUI(eventData.position) || InventoryManager.INSTANCE.CheckInInventoryUI(eventData.position))
            {
                if(eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }
                //判断格子是否重复
                if(targetHolder != InventoryManager.INSTANCE.currentDrag.originalHolder)
                switch(targetHolder.slotType)
                {
                    case SlotType.BAG:
                            if(currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType != ItemType.BUFF)
                            SwapItem();
                        break;
                    case SlotType.ACTION:
                        if (currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.Useable || currentItemUI.Bag.items[currentItemUI.Index].itemData.itemType == ItemType.BUFF)
                        {
                            SwapItem();
                        }
                        break;
                }
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        transform.SetParent(InventoryManager.INSTANCE.currentDrag.originalParent);
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 0;
        t.offsetMin = Vector2.one * 0;
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;
        if(isSameItem && targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }

}
