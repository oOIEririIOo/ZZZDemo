using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    public CharacterStats playerStats;
    public Image icon;
    public Image HP;
    public Image SP;

    public void UpdateHPInfo()
    {
        HP.fillAmount = playerStats.CurrentHealth / playerStats.MaxHealth;
    }
    public void UpdateSPInfo()
    {
        SP.fillAmount = playerStats.CurrentSP / playerStats.MaxSP;
    }
    public void UpdateInfo()
    {
        icon.sprite = playerStats.Icon;
        HP.fillAmount = playerStats.CurrentHealth / playerStats.MaxHealth;
        SP.fillAmount = playerStats.CurrentSP / playerStats.MaxSP;
    }


}
