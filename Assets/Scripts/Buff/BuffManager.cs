using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : SingleMonoBase<BuffManager>
{
    public RectTransform buffPanel;
    public BuffUI buffIcon;

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void UpdateIcon(ItemData_SO buffData)
    {
        var newBuff = Instantiate(buffIcon, buffPanel);
        newBuff.currentBuffData = buffData;
        newBuff.GetComponent<Image>().sprite = buffData.itemIcon;
    }
}
