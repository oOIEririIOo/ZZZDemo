using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXItem : MonoBehaviour
{
    ParticleSystem particle;
    public string vfxName;
    public CharacterNameList characterName;
    bool isInit = false;
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
        particle.Play();
        if(VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName)!= null)
        {
            if(VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos != null)
            {
                transform.position = VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos.position;
                transform.forward = VFXPoolManager.INSTANCE.FindVFXInfo(characterName, vfxName).spawnPos.forward;
            }        
        }
        //TODO: 加入特效列表管理器方便慢放
    }

    private void Update()
    {
        if (!isInit) return;
        if (!particle.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }



    private void OnDisable()
    {
        //VFXManager.INSTANCE.Recycle(vfxName, gameObject);
        VFXPoolManager.INSTANCE.Recycle(characterName, vfxName, gameObject);
        //TODO: 移除特效列表管理器方便慢放
    }
}

public enum VFXType
{
    Slash, Hit
}
