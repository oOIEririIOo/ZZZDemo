using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatsUI : MonoBehaviour
{
    private EnemyController enemyController;
    public GameObject statsUIPrefab;
    public Image HP;
    public Image HPRed;
    public Image stun;
    public Transform UIPoint;
    CharacterStats characterStats;
    public Transform UIbar;
    Transform cam;

    private Coroutine updateRedCoroutinue;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        characterStats = GetComponent<CharacterStats>();
        characterStats.UpdateHealthBarOnAttack += UpdateHealthBar;
        characterStats.UpdateStunBarOnAttack += UpdateStunBar;
    }

    

    private void OnEnable()
    {
        cam = Camera.main.transform;
        UIPoint = enemyController.statsUIpoint;
        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(statsUIPrefab, canvas.transform).transform;
                HP = UIbar.GetChild(0).GetChild(1).GetComponent<Image>();
                HPRed = UIbar.GetChild(0).GetChild(0).GetComponent<Image>();
                stun = UIbar.GetChild(0).GetChild(2).GetComponent<Image>();
                UIbar.gameObject.SetActive(true);
            }
        }
    }

   
    void Update()
    {
        if(!enemyController.isHurt)
        {
            if (updateRedCoroutinue != null)
            {
                StopCoroutine(updateRedCoroutinue);
            }
            updateRedCoroutinue = StartCoroutine(UpdateHPEffect(0.5f));
        }
        else
        {
            if (updateRedCoroutinue != null)
            {
                StopCoroutine(updateRedCoroutinue);
            }
        }
    }


    IEnumerator UpdateHPEffect(float time)
    {
        float redLength = HPRed.fillAmount - HP.fillAmount;
        float currentTime = 0f;

        while(currentTime < time && redLength> 0  && time <= 1f)
        {
            currentTime += Time.deltaTime;
            HPRed.fillAmount = Mathf.Lerp(HP.fillAmount + redLength, HP.fillAmount, currentTime / time);
            yield return null;
        }
        HPRed.fillAmount = HP.fillAmount;
    }
    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = UIPoint.position;
            UIbar.forward = -cam.forward;
        }
    }
    private void UpdateHealthBar(float currentHP, float maxHP)
    {
        if (UIbar != null)
        {
            float sliderPercent = currentHP / maxHP;
            HP.fillAmount = sliderPercent;
        }

        if (currentHP == 0)
        {
            
            UIbar.gameObject.SetActive(false);
            Destroy(UIbar.gameObject, 2f);
            
        }
    }
    private void UpdateStunBar(float currentStun, float maxStun)
    {
        if(UIbar != null)
        {
            float sliderPercent = currentStun / maxStun;
            stun.fillAmount = sliderPercent;
        }
        if(currentStun == maxStun)
        {

        }
    }


}
