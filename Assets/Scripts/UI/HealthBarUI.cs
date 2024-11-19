using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.UI;
using UnityEngine.UI;
using System;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;

    public Transform barPoint;

    Image healthSlider;
    public Transform UIbar;

    Transform cam;
    CharacterStats currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();

        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (UIbar != null)
        {
            float sliderPercent = currentHealth / maxHealth;
            healthSlider.fillAmount = sliderPercent;
        }

        if(currentHealth == 0)
        {
            UIbar.gameObject.SetActive(false);
            Destroy(UIbar.gameObject,5f);
        }
    }

    private void LateUpdate()
    {
        if(UIbar !=null)
        {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;
        }
    }
}
