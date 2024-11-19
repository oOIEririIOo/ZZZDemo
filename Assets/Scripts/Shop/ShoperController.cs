using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoperController : MonoBehaviour
{
    public ShopData_SO shopData;

    bool canTalk = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && shopData != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShopUI.INSTANCE.shopPanel.SetActive(false);
            canTalk = false;
        }
    }

    private void Update()
    {
        if (PlayerController.INSTANCE.inputSystem.Player.Talk.triggered && canTalk)
        {
            OpenShopPanel();
        }
    }

    public void OpenShopPanel()
    {
        //打开面板并更新
        ShopUI.INSTANCE.GetShopData(shopData);
        ShopUI.INSTANCE.SetupGoodList();
    }
}
