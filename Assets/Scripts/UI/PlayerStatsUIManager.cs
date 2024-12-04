using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIManager : SingleMonoBase<PlayerStatsUIManager>
{
    public PlayerStatsUI[] playerStatsUI;

    private protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GetCharacterInfo();
        foreach(var player in playerStatsUI)
        {
            player.UpdateInfo();
        }
    }

    private void Update()
    {
        
    }

   public void UpdatePlayersUI()
    {
        foreach (var player in playerStatsUI)
        {
            player.UpdateInfo();
        }
    }

    public void  SwitchCharacter()
    {
        for (int i = 0; i < PlayerController.INSTANCE.controllableModels.Count; i++)
        {
            int index = PlayerController.INSTANCE.currentModelIndex + i;
            if(PlayerController.INSTANCE.currentModelIndex + i >= PlayerController.INSTANCE.controllableModels.Count)
            {
                index = 0;
            }
            playerStatsUI[i].playerStats = PlayerController.INSTANCE.controllableModels[index].GetComponent<CharacterStats>();
            playerStatsUI[i].UpdateInfo();
        }
    }

    public void GetCharacterInfo()
    {
        for(int i = 0; i < PlayerController.INSTANCE.controllableModels.Count;i++)
        {
            //PlayerController.INSTANCE.characterDic.TryGetValue(PlayerController.INSTANCE.controllableModels[i].GetComponent<CharacterStats>().characterName, out int index);
            playerStatsUI[i].playerStats = PlayerController.INSTANCE.controllableModels[i].GetComponent<CharacterStats>();
        }
    }
}
