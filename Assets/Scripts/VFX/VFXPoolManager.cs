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

        for (int i = 0; i < effectDates.Count; i++)//ѭ����Ч��������
        {
            //�ȴ���һ�������͵��ֵ�
            if (!effectPool.ContainsKey(effectDates[i].style))
            {
                effectPool.Add(effectDates[i].style, new Dictionary<string, Queue<GameObject>>());
                //Debug.Log("�Ѿ�����" + effectDates[i].style + "��Ч�ֵ䣬���ֵ���Ч����"+ effectDates[i].effectItemData.effectItems.Count);
            }

            for (int j = 0; j < effectDates[i].effectItemData.effectItems.Count; j++)//ѭ��ÿ����Ч�����еĶ����Ŀ
            {
                //effectDates[i].effectItemData.effectItems[j].effectRotation = Quaternion.Euler(effectDates[i].effectItemData.effectItems[j].effectEulerAngle);

                for (int k = 0; k < effectDates[i].effectItemData.effectItems[j].count; k++)
                {
                    //����ʵ��,��ʼ����Ч����
                    GameObject go = Instantiate(effectDates[i].effectItemData.effectItems[j].VFXPrefab);
                   
                    if (!go.TryGetComponent<VFXItem>(out var vFXItem))
                    {
                        vFXItem = go.AddComponent<VFXItem>();    
                    }
                    vFXItem.Init(effectDates[i].style, effectDates[i].effectItemData.effectItems[j].VFXName);   
                  

                    if (effectDates[i].effectItemData.effectItems[j].applyParentPos)
                    {
                        //���ø�����
                        if(effectDates[i].effectItemData.effectItems[j].parentPos != null)
                        {
                            go.transform.parent = effectDates[i].effectItemData.effectItems[j].parentPos;
                        }
                        else
                        {
                            //go.transform.parent = PlayerController.INSTANCE.vfxPos[PlayerController.INSTANCE.currentModelIndex].transform;
                            if (PlayerController.INSTANCE.characterDic.TryGetValue(effectDates[i].style, out int index))
                            {
                                go.transform.parent = PlayerController.INSTANCE.vfxPos[index].transform;
                            }
                            else if(effectDates[i].style == CharacterNameList.Enemy)
                            {
                                go.transform.parent = this.transform;
                                //Debug.Log("�ֵ�Ϊ�գ�Ҳ�����Ч����Enemy");
                            }

                            
                        }
                    }
                    else
                    {
                        go.transform.parent = this.transform;
                    }
                    //λ��
                    go.transform.localPosition = Vector3.zero;
                    //��ת
                    //go.transform.localRotation = effectDates[i].effectItemData.effectItems[j].effectRotation;
                    
                    if(effectDates[i].effectItemData.effectItems[j].spawnPos != null)
                    {
                        go.transform.position = effectDates[i].effectItemData.effectItems[j].spawnPos.position;
                    }
                    else
                    {
                        if (PlayerController.INSTANCE.characterDic.TryGetValue(effectDates[i].style, out int index))
                        {
                            go.transform.position = PlayerController.INSTANCE.vfxPos[index].transform.position;
                        }
                        else if (effectDates[i].style == CharacterNameList.Enemy)
                        {
                            go.transform.parent = this.transform;
                            //Debug.Log("�ֵ�Ϊ�գ�Ҳ�����Ч����Enemy");
                        }
                        
                    }
                    
                    //����
                    go.SetActive(false);
                    //�����ֵ�
                    if (!effectPool[effectDates[i].style].ContainsKey(effectDates[i].effectItemData.effectItems[j].VFXName))
                    {
                        effectPool[effectDates[i].style].Add(effectDates[i].effectItemData.effectItems[j].VFXName, new Queue<GameObject>());
                        //Debug.Log(effectDates[i].effectItemData.effectItems[j].VFXName + "�ѷ���" + effectDates[i].effectItemData + "�ֵ�");
                    }
                    effectPool[effectDates[i].style][effectDates[i].effectItemData.effectItems[j].VFXName].Enqueue(go);
                }
            }
        }
    }


    /// <summary>
    /// ֻ�������и�������Ч
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="effectName"></param>
    public void TryGetVFX(CharacterNameList characterName, string effectName)
    {
        
        if(effectPool[characterName][effectName].Count == 0)
        {
            var vfx = FindVFXInfo(characterName, effectName);
            GameObject go = Instantiate(vfx.VFXPrefab);
            //��ʼ����Ч
            if (!go.TryGetComponent<VFXItem>(out var vFXItem))
            {
                vFXItem = go.AddComponent<VFXItem>();
            }
            vFXItem.Init(characterName, effectName);
            if (vfx.applyParentPos)
            {
                //���ø�����
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

            if(vfx.spawnPos != null && !vfx.applyParentPos)
            {
                go.transform.position = vfx.spawnPos.position;
                //go.transform.localPosition = Vector3.zero;
                go.transform.forward = vfx.spawnPos.forward;
            }
            else if(!vfx.applyParentPos)
            {
                if (PlayerController.INSTANCE.characterDic.TryGetValue(characterName, out int index))
                {
                    go.transform.parent = this.transform;
                    go.transform.position = PlayerController.INSTANCE.vfxPos[index].transform.position;
                    go.transform.forward = PlayerController.INSTANCE.vfxPos[index].transform.forward;
                }
                else Debug.Log("�ֵ�Ϊ�գ�Ҳ�����Ч����Enemy");
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
            Debug.LogWarning(characterName + "����" + effectName + "���ֵ�" + "����ز�����");
        }
    }

    public void TryGetVFX(CharacterNameList characterName, string effectName,Transform pos)
    {

        if (effectPool[characterName][effectName].Count == 0)
        {
            var vfx = FindVFXInfo(characterName, effectName);
            GameObject go = Instantiate(vfx.VFXPrefab);
            //��ʼ����Ч
            if (!go.TryGetComponent<VFXItem>(out var vFXItem))
            {
                vFXItem = go.AddComponent<VFXItem>();
            }
            vFXItem.Init(characterName, effectName);
            go.transform.parent = pos;
            vFXItem.transform.localPosition = Vector3.zero;
            vFXItem.transform.forward = PlayerController.INSTANCE.characterInfo[PlayerController.INSTANCE.currentModelIndex].GetComponent<PlayerModel>().transform.forward;
            effectPool[characterName][effectName].Enqueue(go);

        }

        if (effectPool.ContainsKey(characterName) && effectPool[characterName].ContainsKey(effectName) && effectPool[characterName][effectName].Count > 0)
        {
            GameObject go = effectPool[characterName][effectName].Dequeue();
            if (!go.TryGetComponent<VFXItem>(out var vFXItem))
            {
                vFXItem = go.AddComponent<VFXItem>();
            }
            vFXItem.SetParent(pos);
            vFXItem.transform.localPosition = Vector3.zero;
            vFXItem.transform.forward = PlayerController.INSTANCE.characterInfo[PlayerController.INSTANCE.currentModelIndex].GetComponent<PlayerModel>().transform.forward;
            go.SetActive(true);
        }
        else
        {
            Debug.LogWarning(characterName + "����" + effectName + "���ֵ�" + "����ز�����");
        }
    }

    public void TryGetVFXByEnemy(CharacterNameList characterName, string effectName,Transform pos)
    {

        if (effectPool[characterName][effectName].Count == 0)
        {
            var vfx = FindVFXInfo(characterName, effectName);
            GameObject go = Instantiate(vfx.VFXPrefab);
            //��ʼ����Ч
            if (!go.TryGetComponent<VFXItem>(out var vFXItem))
            {
                vFXItem = go.AddComponent<VFXItem>();
            }
            vFXItem.Init(characterName, effectName);
            if (vfx.applyParentPos)
            {
                //���ø�����
                if (vfx.applyParentPos)
                {
                        go.transform.parent = pos;
                        go.transform.localPosition = Vector3.zero;
                        go.transform.forward = pos.forward;
                }
            }
            else
            {
                go.transform.position = pos.position;
                go.transform.forward = pos.forward;
            }

           
            effectPool[characterName][effectName].Enqueue(go);

        }

        if (effectPool.ContainsKey(characterName) && effectPool[characterName].ContainsKey(effectName) && effectPool[characterName][effectName].Count > 0)
        {
            GameObject go = effectPool[characterName][effectName].Dequeue();
            go.SetActive(true);
            var vfx = FindVFXInfo(characterName, effectName);
            //��ʼ����Ч
            if (!go.TryGetComponent<VFXItem>(out var vFXItem))
            {
                vFXItem = go.AddComponent<VFXItem>();
            }
            vFXItem.Init(characterName, effectName);
            if (vfx.applyParentPos)
            {
                //���ø�����
                if (vfx.applyParentPos)
                {
                    go.transform.parent = pos;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.forward = pos.forward;
                }
            }
            else
            {
                go.transform.position = pos.position;
                go.transform.forward = pos.forward;
            }

        }
        else
        {
            Debug.LogWarning(characterName + "����" + effectName + "���ֵ�" + "����ز�����");
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

    public void SpawnHitVfx(CharacterNameList characterName,AttackInfo attackInfo,Vector3 spawnPos,Vector3 forward)
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
            //��ʼ����Ч
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
            Debug.LogWarning(characterName + "����" + effectName + "���ֵ�" + "����ز�����");
        }
    }


    /// <summary>
    /// ��������û�и�������Ч
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
            Debug.LogWarning(characterName + "����" + effectName + "���ֵ�" + "����ز�����");
        }

    }

}
