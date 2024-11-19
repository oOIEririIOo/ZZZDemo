using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SaveManager : SingleMonoBase<SaveManager>
{
    string sceneName = "";

    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Y))
        {
            SavePlayerData();
        }

        if (Input.GetKey(KeyCode.U))
        {
            LoadPlayerData();
        }
    }

    public void SavePlayerData()
    {
        foreach(GameObject player in PlayerController.INSTANCE.characterInfo)
        {
            Save(player.GetComponent<CharacterStats>().characterData, player.GetComponent<CharacterStats>().characterData.name);
        }
    }

    public void LoadPlayerData()
    {
        foreach (GameObject player in PlayerController.INSTANCE.characterInfo)
        {
            Load(player.GetComponent<CharacterStats>().characterData, player.GetComponent<CharacterStats>().characterData.name);
        }
    }

    public void Save(Object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key,jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }

    public void Load(Object data, string key)
    {
        if(PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }

    }

    public void SaveNewGamePlayerData()
    {
        foreach (GameObject player in PlayerController.INSTANCE.characterInfo)
        {
            Save(player.GetComponent<CharacterStats>().characterData, player.GetComponent<CharacterStats>().characterData.name + "NewGamePlayer");
        }
    }

    public void LoadNewGamePlayerData()
    {
        foreach (GameObject player in PlayerController.INSTANCE.characterInfo)
        {
            Load(player.GetComponent<CharacterStats>().characterData, player.GetComponent<CharacterStats>().characterData.name + "NewGamePlayer");
        }
    }

}
