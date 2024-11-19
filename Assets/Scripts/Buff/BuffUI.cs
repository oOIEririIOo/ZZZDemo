using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class BuffUI : MonoBehaviour
{
    public ItemData_SO currentBuffData;
    public float timer;
    private void Start()
    {
        timer = currentBuffData.buffData.durationTime;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        
        if(timer <= 0f)
        {
            BuffOFF();
            Destroy(this.gameObject);
        }
        
    }

    private void BuffOFF()
    {
        foreach (var model in PlayerController.INSTANCE.characterInfo)
            currentBuffData.buffData.BUFFOff(model.GetComponent<PlayerModel>().characterStats.skillConfig, model.GetComponent<PlayerModel>().characterStats);
    }
}
