using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemUI : MonoBehaviour
{
    public Image icon = null;
    public TMP_Text amount = null;
    public ItemData_SO currentItemData;
    public Text price = null;

    public InventoryData_SO Bag { get; set; }
    
    public int Index { get; set; } = -1;

    public void SetupItemUI(ItemData_SO item,int itemAmount)
    {

        if(itemAmount == 0)
        {
            Bag.items[Index].itemData = null;
            icon.gameObject.SetActive(false);
            return;
        }

        if(itemAmount <0)
        {
            item = null;
        }

        if(item != null)
        {
            currentItemData = item;
            icon.sprite = item.itemIcon;
            amount.text = itemAmount.ToString();
            icon.gameObject.SetActive(true);
        }
        else
            icon.gameObject.SetActive(false);
    }

    public ItemData_SO GetItem()
    {
        return Bag.items[Index].itemData;
    }

    public void OpenReconfirmPanel()
    {
        ShopUI.INSTANCE.reconfirmPanel.SetActive(true);
    }

    public void TheItemInfo()
    {
        ShopUI.INSTANCE.ChooseItem(this);
    }

}
