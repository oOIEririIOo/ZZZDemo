using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;
    private SlotHolder currentSlotHolder;

    private void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(actionKey) && currentSlotHolder.itemUI.GetItem() && currentSlotHolder.itemUI.GetItem().itemType == ItemType.Useable)
        {
            currentSlotHolder.UseItem();
        }
        if(Input.GetKeyDown(actionKey) && currentSlotHolder.itemUI.GetItem() && currentSlotHolder.itemUI.GetItem().itemType == ItemType.BUFF)
        {
            currentSlotHolder.UseBuff();
        }
    }
}
