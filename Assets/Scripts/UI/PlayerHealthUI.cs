using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    Image healthSlider;
    private void Awake()
    {
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        //DontDestroyOnLoad(this);
    }

    private void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        float sliderPercent = (float)PlayerController.INSTANCE.playerModel.characterStats.CurrentHealth / PlayerController.INSTANCE.playerModel.characterStats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    
}
