using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TransitionDestination;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : SingleMonoBase<GameManager>
{


    List<ISwitichScene> switichScenes = new List<ISwitichScene>();

    private protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        /*
        if(PlayerController.INSTANCE.mouseOpen)
        {
            Time.timeScale = 0f;
        }
        else Time.timeScale = 1f;
        */
    }

    public void AddObserver(ISwitichScene observer)
    {
        switichScenes.Add(observer);
    }

    public void RemoveObserver(ISwitichScene observer)
    {
        if(switichScenes.Count != 0)
        switichScenes.Remove(observer);
    }

    public void  NotifyObservers()
    {
        foreach(var observer in switichScenes)
        {
            observer.OnSwitchScene();
        }
    }

    public Transform GetEntrance()
    {
        foreach(var item in FindObjectsByType<TransitionDestination>(FindObjectsSortMode.None))
        {
            if(item.destinationTag == TransitionDestination.DestinationTag.ENTER)
            {
                Debug.Log(item.transform);
                return item.transform;
                
            }
        }
        return null;
    }
}
