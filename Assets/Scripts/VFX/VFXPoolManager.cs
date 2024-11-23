using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VFXItemData;

public class VFXPoolManager : SingleMonoBase<VFXPoolManager>
{ 
    [System.Serializable]
    public class effectData
    {
        public CharacterNameList style;
        public VFXItemData effectItemData;
    }

    [SerializeField] private List<effectData> effectDates = new List<effectData>();
    private Dictionary<CharacterNameList, Dictionary<string, Queue<GameObject>>> effectPool = new Dictionary<CharacterNameList, Dictionary<string, Queue<GameObject>>>();

    private protected override void Awake()
    {
        base.Awake();
        InitEffectPools();
    }
    private void Start()
    {
        
    }

    private void InitEffectPools()
    {
        if (effectDates.Count == 0) { return; }

        for (int i = 0; i < effectDates.Count; i++)//循环特效数据类型
        {
            //先创建一个该类型的字典
            if (!effectPool.ContainsKey(effectDates[i].style))
            {
                effectPool.Add(effectDates[i].style, new Dictionary<string, Queue<GameObject>>());
            }

            for (int j = 0; j < effectDates[i].effectItemData.effectItems.Count; j++)//循环每个特效类型中的多个项目
            {
                //effectDates[i].effectItemData.effectItems[j].effectRotation = Quaternion.Euler(effectDates[i].effectItemData.effectItems[j].effectEulerAngle);

                for (int k = 0; k < effectDates[i].effectItemData.effectItems[j].count; k++)
                {
                    //创建实例,初始化特效对象
                    GameObject go = Instantiate(effectDates[i].effectItemData.effectItems[j].VFXPrefab);
                    if (!go.TryGetComponent<VFXItem>(out var vFXItem))
                    {
                        vFXItem = go.AddComponent<VFXItem>();
                    }
                    vFXItem.Init(effectDates[i].style, effectDates[i].effectItemData.effectItems[j].VFXName);
                    if (effectDates[i].effectItemData.effectItems[j].applyParentPos)
                    {
                        //设置父级点
                        if(effectDates[i].effectItemData.effectItems[j].parentPos != null)
                        {
                            go.transform.parent = effectDates[i].effectItemData.effectItems[j].parentPos;
                        }
                        else
                        {
                            //go.transform.parent = PlayerController.INSTANCE.vfxPos[PlayerController.INSTANCE.currentModelIndex].transform;
                            if(PlayerController.INSTANCE.characterDic.TryGetValue(effectDates[i].style,out int index))
                            {
                                go.transform.parent = PlayerController.INSTANCE.vfxPos[index].transform;
                            }
                            
                        }
                    }
                    else
                    {
                        go.transform.parent = this.transform;
                    }
                    //位置
                    go.transform.localPosition = Vector3.zero;
                    //旋转
                    //go.transform.localRotation = effectDates[i].effectItemData.effectItems[j].effectRotation;
                    
                    if(effectDates[i].effectItemData.effectItems[j].spawnPos != null)
                    {
                        go.transform.position = effectDates[i].effectItemData.effectItems[j].spawnPos.position;
                    }
                    
                    //隐藏
                    go.SetActive(false);
                    //放入字典
                    if (!effectPool[effectDates[i].style].ContainsKey(effectDates[i].effectItemData.effectItems[j].VFXName))
                    {
                        effectPool[effectDates[i].style].Add(effectDates[i].effectItemData.effectItems[j].VFXName, new Queue<GameObject>());
                    }
                    effectPool[effectDates[i].style][effectDates[i].effectItemData.effectItems[j].VFXName].Enqueue(go);
                }
            }
        }
    }


    /// <summary>
    /// 只能设置有父级的特效
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="effectName"></param>
    public void TryGetVFX(CharacterNameList characterName, string effectName)
    {
        
        if(effectPool[characterName][effectName].Count == 0)
        {
            var vfx = FindVFXInfo(characterName, effectName);
            GameObject go = Instantiate(vfx.VFXPrefab);
            //初始化特效
            if (!go.TryGetComponent<VFXItem>(out var vFXItem))
            {
                vFXItem = go.AddComponent<VFXItem>();
            }
            vFXItem.Init(characterName, effectName);
            if (vfx.applyParentPos)
            {
                //设置父级点
                if (vfx.applyParentPos)
                {
                    if(vfx.parentPos != null)
                    {
                        go.transform.parent = vfx.parentPos;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.forward = vfx.parentPos.forward;
                    }
                    else
                    {
                        go.transform.parent = PlayerController.INSTANCE.vfxPos[PlayerController.INSTANCE.currentModelIndex].transform;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.forward = PlayerController.INSTANCE.vfxPos[PlayerController.INSTANCE.currentModelIndex].transform.forward;
                    }
                    
                }  
            }
            else
            {
                go.transform.parent = this.transform;
            }

            if(vfx.spawnPos != null)
            {
                go.transform.position = vfx.spawnPos.position;
                //go.transform.localPosition = Vector3.zero;
                go.transform.forward = vfx.spawnPos.forward;
            }
            effectPool[characterName][effectName].Enqueue(go);

        }
        
        if (effectPool.ContainsKey(characterName) && effectPool[characterName].ContainsKey(effectName) && effectPool[characterName][effectName].Count > 0)
        {
            GameObject go = effectPool[characterName][effectName].Dequeue();
            go.SetActive(true);
        }
        else
        {
            Debug.LogWarning(characterName + "类型" + effectName + "名字的" + "对象池不存在");
        }
    }

    public void Recycle(CharacterNameList characterName,string name, GameObject vfxGO)
    {

        if (effectPool[characterName].TryGetValue(name, out var queue))
        {
            queue.Enqueue(vfxGO);
        }
    }

    public EffectItem FindVFXInfo(CharacterNameList characterName, string effectName)
    {
        foreach (var eff in effectDates)
        {
            var vfx = eff.effectItemData.effectItems.Find(i => i.VFXName == effectName);
            if (vfx != null)
            {
                return vfx;
            }
        }

        return null; 
        
    }

    public void SpawnHitVfx(CharacterNameList characterName,AttackInfo attackInfo,Vector3 spawnPos,Vector3 forward,int index)
    {
        string effectName = attackInfo.hitInfo[attackInfo.hitIndex].hitVFX.VFXPrefab.gameObject.name;

        if (!effectPool.ContainsKey(characterName))
        {
            effectPool.Add(characterName, new Dictionary<string, Queue<GameObject>>());
            
        }
        if (!effectPool[characterName].ContainsKey(effectName))
        {
            effectPool[characterName].Add(effectName, new Queue<GameObject>());
            attackInfo.hitInfo[attackInfo.hitIndex].hitVFX.parentPos = null;
            attackInfo.hitInfo[attackInfo.hitIndex].hitVFX.spawnPos = null;
        }

        if (effectPool[characterName][effectName].Count == 0)
        {
            GameObject go = Instantiate(attackInfo.hitInfo[attackInfo.hitIndex].hitVFX.VFXPrefab);
            //初始化特效
            if (!go.TryGetComponent<VFXItem>(out var vFXItem))
            {
                vFXItem = go.AddComponent<VFXItem>();
            }
            vFXItem.Init(characterName, effectName);
            go.transform.parent = this.transform;
            go.transform.position = spawnPos;
            effectPool[characterName][effectName].Enqueue(go);
            
        }

        if (effectPool.ContainsKey(characterName) && effectPool[characterName].ContainsKey(effectName) && effectPool[characterName][effectName].Count > 0)
        {
            GameObject go = effectPool[characterName][effectName].Dequeue();
            go.transform.position = spawnPos;
            go.transform.forward = forward;
            go.SetActive(true);
        }
        else
        {
            Debug.LogWarning(characterName + "类型" + effectName + "名字的" + "对象池不存在");
        }
    }


    /// <summary>
    /// 用来设置没有父级的特效
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="effectName"></param>
    /// <param name="worldPos"></param>
    /// <param name="quaternion"></param>
    public void GetVFX(CharacterNameList characterName, string effectName, Vector3 worldPos = default(Vector3), Quaternion quaternion = default(Quaternion))
    {
        if (effectPool.ContainsKey(characterName) && effectPool[characterName].ContainsKey(effectName) && effectPool[characterName][effectName].Count > 0)
        {
            GameObject go = effectPool[characterName][effectName].Dequeue();
            go.transform.position = worldPos;
            go.transform.rotation = quaternion;
            go.SetActive(true);
            effectPool[characterName][effectName].Enqueue(go);
        }
        else
        {
            Debug.LogWarning(characterName + "类型" + effectName + "名字的" + "对象池不存在");
        }

    }

}
