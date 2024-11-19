using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button exitBtn;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        exitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(NewGame);
        continueBtn.onClick.AddListener(ContinueGame);
        exitBtn.onClick.AddListener(QuitGame);
    }

    void NewGame()
    {

        SaveManager.INSTANCE.LoadNewGamePlayerData();
        PlayerPrefs.DeleteAll();
        SaveManager.INSTANCE.SaveNewGamePlayerData();

        //转换场景
        SceneController.INSTANCE.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        //转换场景 读取进度
        SceneController.INSTANCE.TransitionToLoadGame();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
