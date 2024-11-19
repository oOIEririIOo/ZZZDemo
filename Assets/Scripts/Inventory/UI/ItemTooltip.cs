using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    public Text itemNameText;
    public Text itemInfoText;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupTooltip(ItemData_SO item)
    {
        itemNameText.text = item.itemName;
        itemInfoText.text = item.description;
    }

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void Update()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        Vector3 mousPos = Input.mousePosition;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        if(mousPos.y < height)
        {
            rectTransform.position = mousPos + Vector3.up * height * 0.65f;
        }
        else if(Screen.width - mousPos.x >width)
        {
            rectTransform.position = mousPos + Vector3.right * width * 0.65f;
        }
        else
        {
            rectTransform.position = mousPos + Vector3.left * width * 0.65f;
        }

    }
}
