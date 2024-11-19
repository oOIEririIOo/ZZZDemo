using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : SingleMonoBase<ShopUI>
{
    public ShopData_SO currentShop;
    
    public GameObject shopPanel;
    public GameObject reconfirmPanel;
    public GameObject cantBuy;

    public RectTransform goods;
    public ItemUI goodsButtonUI;

    public ItemData_SO currentItem;
    public int currentPrice;
    public ItemData_SO myCoins;

    public void GetShopData(ShopData_SO currentShopData)
    {
        currentShop = currentShopData;
    }

    public void SetupGoodList()
    {
        shopPanel.SetActive(true);
        foreach(Transform goodBTN in goods)
        {
            Destroy(goodBTN.gameObject);
        }
        foreach(var good in currentShop.goodLists)
        {
            var newGood = Instantiate(goodsButtonUI, goods);
            newGood.icon.sprite = good.itemData.itemIcon;
            newGood.price.text = good.price.ToString();
            newGood.currentItemData = good.itemData;
        }
    }

    public void ChooseItem(ItemUI item)
    {
        currentItem = item.currentItemData;
        currentPrice = int.Parse(item.price.text);
    }

    
    public void BuyItem()
    {
        if(InventoryManager.INSTANCE.QuestItemInBag(myCoins) != null)
        {
            if(InventoryManager.INSTANCE.QuestItemInBag(myCoins).amount >= currentPrice)
            {
                InventoryManager.INSTANCE.inventoryData.AddItem(currentItem, 1);
                InventoryManager.INSTANCE.QuestItemInBag(myCoins).amount -= currentPrice;
            }
        }
        else
        {
            cantBuy.SetActive(true);
        }
        
        InventoryManager.INSTANCE.inventoryUI.RefreshUI();
        InventoryManager.INSTANCE.actionUI.RefreshUI();
    }
    
}
