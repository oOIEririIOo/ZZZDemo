using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFXItem : MonoBehaviour
{
    ParticleSystem particle;
    public string vfxName;
    public CharacterNameList characterName;
    bool isInit = false;
    public float originalDuration;
    private float timer;
    public Transform spawnPoint;
    private void Awake()
    {
        particle = transform.GetComponent<ParticleSystem>();
    }

    public void Init(CharacterNameList characterName, string effectName,Transform pos)
    {
        isInit = true;
        this.vfxName = effectName;
        this.characterName = characterName;
        VFXManager.INSTANCE.AddVFX(particle, 1f);
        spawnPoint = pos;
    }
    public void SetParent(Transform pos)
    {
        this.transform.parent = pos;
    }
    private void OnEnable()
    {
        if (!isInit) return;
        Debug.Log("Dead");
        timer = 0f;
        particle.Simulate(0f);
        particle.Play();
        if (VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName) != null)
        {

            if (VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos != null)
            {
                transform.position = VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos.position;
                transform.forward = VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos.forward;
            }
            else if(VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).applyParentPos)
            {
                transform.parent = spawnPoint;
                transform.localPosition = Vector3.zero;
                transform.forward = spawnPoint.forward;
            }
            else
            {
                transform.position = spawnPoint.position;
                transform.forward = spawnPoint.forward;
            }
        }
        //TODO: 加入特效列表管理器方便慢放
    }

    private void Update()
    {
        //timer += Time.deltaTime;

        if (!isInit) return;
        if (!particle.isPlaying)
        {
            gameObject.SetActive(false);
        }

        /*
         if(timer >= particle.main.duration)
         {
             gameObject.SetActive(false);
         }
        */
    }



    private void OnDisable()
    {
        //VFXManager.INSTANCE.Recycle(vfxName, gameObject);
        particle.Stop();
        gameObject.SetActive(false);
        VFXPoolManager.INSTANCE.Recycle(characterName, vfxName, gameObject);
        //TODO: 移除特效列表管理器方便慢放
    }
    public void FindChild()
    {
        var allChild = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChild)
        {
            Debug.Log(child.name);
        }
    }

}
