using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.PlayerSettings;

public class SceneController : SingleMonoBase<SceneController>
{
    public GameObject playerPrefab;
    GameObject player;
    GameObject enemies;
    PlayerConfig playerConfig;

    public SceneFader sceneFaderPrefab;

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void TPtoDestination(TransitionPoint transitionPoint)
    {
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTag));
                break;
            case TransitionPoint.TransitionType.DiffScene:
                StartCoroutine(Transition(transitionPoint.sceneName, transitionPoint.destinationTag));
                break;

        }
    }
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        //保存数据
        SaveManager.INSTANCE.SavePlayerData();
        InventoryManager.INSTANCE.SaveData();
        QuestManager.INSTANCE.SaveQuestManager();

        if(SceneManager.GetActiveScene().name != sceneName)
        {
            yield return StartCoroutine(fade.FadeOut(0.5f));
            GameManager.INSTANCE.NotifyObservers();
            //AllEnemyController.INSTANCE.GetComponent<AllEnemyController>().FindEnemy();
            //enemies = AllEnemyController.INSTANCE.enemies.gameObject;
            enemies.SetActive(false);
            

            player = PlayerController.INSTANCE.playerModel.gameObject;
            player.GetComponent<CharacterController>().enabled = false;
            yield return SceneManager.LoadSceneAsync(sceneName);
            SaveManager.INSTANCE.SavePlayerData();
            //InventoryManager.INSTANCE.SaveData();
            //QuestManager.INSTANCE.SaveQuestManager();



            //读取数据
            SaveManager.INSTANCE.LoadPlayerData();
            //InventoryManager.INSTANCE.LoadData();





            //yield return Instantiate(playerPrefab);
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            player.GetComponent<CharacterController>().enabled = true;
            SaveManager.INSTANCE.LoadPlayerData();

            //AllEnemyController.INSTANCE.GetComponent<AllEnemyController>().FindEnemy();
            //enemies = AllEnemyController.INSTANCE.enemies.gameObject;
            enemies.SetActive(true);

            yield return StartCoroutine(fade.FadeIn(2f));

            yield break;
        }
        else
        {
            yield return StartCoroutine(fade.FadeOut(0.5f));
            player = PlayerController.INSTANCE.playerModel.gameObject;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            player.GetComponent<CharacterController>().enabled = true;
            Debug.Log(destinationTag);
            yield return StartCoroutine(fade.FadeIn(2f));
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsByType<TransitionDestination>(FindObjectsSortMode.None);
        for(int i=0;i<entrances.Length;i++)
        {
            if (entrances[i].destinationTag == destinationTag)
            {
                return entrances[i];
            }
        }
        return null;
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManager.INSTANCE.SceneName));
        SaveManager.INSTANCE.LoadPlayerData();
        //InventoryManager.INSTANCE.LoadData();
        //QuestManager.INSTANCE.LoadQuestManager();
    }


    public void TransitionToMain()
    {

        StartCoroutine(LoadMain());
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("01"));
    }

    IEnumerator LoadLevel(string scene)
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        if (scene !="")
        {
            yield return StartCoroutine(fade.FadeOut(0.5f));
            GameManager.INSTANCE.NotifyObservers();
            yield return SceneManager.LoadSceneAsync(scene);
            player = PlayerController.INSTANCE.playerModel.gameObject;
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.SetPositionAndRotation(GameManager.INSTANCE.GetEntrance().position, GameManager.INSTANCE.GetEntrance().rotation);
            player.GetComponent<CharacterController>().enabled = true;

           
            //SaveManager.INSTANCE.LoadPlayerData();
            //InventoryManager.INSTANCE.LoadData();
            yield return StartCoroutine(fade.FadeIn(2f));
            //保存数据
            SaveManager.INSTANCE.SavePlayerData();
            InventoryManager.INSTANCE.SaveData(); 
            yield break;
               
        }

    }

    IEnumerator LoadMain()
    {
        GameManager.INSTANCE.NotifyObservers();
        yield return SceneManager.LoadSceneAsync("Main");
        yield break;
    }

}
