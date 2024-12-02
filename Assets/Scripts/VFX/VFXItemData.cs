using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXItemData : MonoBehaviour
{
    [System.Serializable]
    public class EffectItem
    {

        [field: SerializeField] public string VFXName;
        [field: SerializeField] public GameObject VFXPrefab;
        [field: SerializeField] public bool applyParentPos;
        [field: SerializeField] public Transform parentPos;
        [field: SerializeField] public Transform spawnPos;
        [field: SerializeField] public Vector3 effectEulerAngle;
        [field: SerializeField] public Quaternion effectRotation;
        [field: SerializeField] public int count = 1;
    }

    [SerializeField] public List<EffectItem> effectItems = new List<EffectItem>();

}
