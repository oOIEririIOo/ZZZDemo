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

        //��¼ԭʼ����
        transform.SetParent(InventoryManager.INSTANCE.dragCanvas.transform, true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        //�������λ��
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //������Ʒ ��������
        // �Ƿ�ָ��UI��Ʒ
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
                //�жϸ����Ƿ��ظ�
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
