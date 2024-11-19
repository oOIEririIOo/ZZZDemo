using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ItemUI currentItemUI;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        QuestUI.INSTANCE.tooltip.gameObject.SetActive(true);
        QuestUI.INSTANCE.tooltip.SetupTooltip(currentItemUI.currentItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        QuestUI.INSTANCE.tooltip.gameObject.SetActive(false);
    }
}
