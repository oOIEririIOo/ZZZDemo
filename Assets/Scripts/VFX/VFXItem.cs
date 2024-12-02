using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXItem : MonoBehaviour
{
    ParticleSystem particle;
    public string vfxName;
    public CharacterNameList characterName;
    bool isInit = false;
    private float timer;
    private void Awake()
    {
        particle = transform.GetComponent<ParticleSystem>(); 
    }

    public void Init(CharacterNameList characterName, string effectName)
    {
        isInit = true;
        this.vfxName = effectName;
        this.characterName = characterName;
        VFXManager.INSTANCE.AddVFX(particle, 1f);
        
    }
    public void SetParent(Transform pos)
    {
        this.transform.parent = pos;
    }
    private void OnEnable()
    {
        if (!isInit) return;
        timer = 0f;
        particle.Simulate(0f);
        particle.Play();
        
        if (VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName)!= null)
        {
            if(VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos != null)
            {
                transform.position = VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos.position;
                transform.forward = VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos.forward;
            }
            else
            {
                if (PlayerController.INSTANCE.characterDic.TryGetValue(characterName, out int index))
                { 
                    transform.position = PlayerController.INSTANCE.vfxPos[index].transform.position;
                    transform.forward = PlayerController.INSTANCE.vfxPos[index].transform.forward;
                }
                else Debug.Log("字典为空！也许该特效属于Enemy");
            }
        }
        //TODO: 加入特效列表管理器方便慢放
    }

    private void Update()
    {
        timer += Time.deltaTime;
        /*
        //if (!isInit) return;
        Debug.Log(particle.main.duration);
        if (!particle.isPlaying)
        {
            gameObject.SetActive(false);
        }
        */
       
        if(timer >= particle.main.duration)
        {
            gameObject.SetActive(false);
        }
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

public enum VFXType
{
    Slash, Hit
}
