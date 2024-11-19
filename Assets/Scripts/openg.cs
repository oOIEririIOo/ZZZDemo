using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Openg : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(ToScene01());
    }

    IEnumerator ToScene01()
    {
        yield return SceneManager.LoadSceneAsync("Main");
    }


}
