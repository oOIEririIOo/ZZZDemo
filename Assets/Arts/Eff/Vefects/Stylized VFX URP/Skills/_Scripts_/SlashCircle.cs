using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashCircle : MonoBehaviour
{
    // Start is called before the first frame update

    float slashCircleTime;
    float slashCircleRate;
    AudioSource SFX_SlashCircle;
    GameObject SFX_GameobjectHolder;

    void Awake()
    {
        SFX_SlashCircle = gameObject.GetComponent<AudioSource>();
        slashCircleTime = 1.1f;
        slashCircleRate = 0.18f;

    }

    // Update is called once per frame
    void Update()
    {
        if (slashCircleTime > 0)
        {
            slashCircleTime -= Time.deltaTime;

            if (slashCircleRate > 0)
            {
                slashCircleRate -= Time.deltaTime;
            }
            else if (slashCircleRate <= 0)
            {
                //Instantiate(SFX_GameobjectHolder, transform.position, transform.rotation);
                //SFX_GameobjectHolder.AddComponent<AudioSource>();
                //SFX_GameobjectHolder.GetComponent<AudioSource>().clip = SFX_SlashCircle.clip;
                slashCircleRate = 0.18f;
            }
        }
        else if (slashCircleTime <= 0)
        {
            slashCircleTime = 0;
        }
    }
}
