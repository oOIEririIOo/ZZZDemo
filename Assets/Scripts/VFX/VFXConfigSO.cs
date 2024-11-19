using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New VFX",menuName ="VFXdata")]
public class VFXConfigSO : ScriptableObject
{
    public List<VFXItemInfo> vFXItemInfos;
}
[System.Serializable]
public class VFXItemInfo
{
    public string vfxName;
    public GameObject prefab;
    public int reloadCount;
}
