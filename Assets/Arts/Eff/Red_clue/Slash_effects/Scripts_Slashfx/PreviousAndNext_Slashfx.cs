using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousAndNext_Slashfx : MonoBehaviour
{
 
    public GameObject[] Prefab;
    private int number;
    private GameObject currentInstance;

    void Start()
    {
        ChangeCurrent(0);
    }

    bool IsPressed;

    void OnGUI()
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            IsPressed = false;
        
        if (!IsPressed && Input.GetKeyDown(KeyCode.A))
        {
            IsPressed = true;
            ChangeCurrent(-1);
        }

        if (!IsPressed && Input.GetKeyDown(KeyCode.D))
        {
            IsPressed = true;
            ChangeCurrent(+1);
        }
    }

    void ChangeCurrent(int delta)
    {
        number += delta;
        if (number > Prefab.Length - 1)
            number = 0;
        else if (number < 0)
            number = Prefab.Length - 1;

        if (currentInstance != null)
        {
            Destroy(currentInstance);
            
        }

        currentInstance = Instantiate(Prefab[number]);

    }
}
