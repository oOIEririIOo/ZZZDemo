using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LootSpawner : MonoBehaviour
{

    [System.Serializable]
    public class LootItem
    {
        public GameObject item;

        [Range(0,1)]
        public float weight;

    }

    public LootItem[]  lootItems;

    public void SpawnLoot()
    {
        float currentValue = Random.value;
        for (int i = 0; i < lootItems.Length; i++)
        {
            if(currentValue<= lootItems[i].weight)
            {
                GameObject obj = Instantiate(lootItems[i].item);
                obj.transform.position = transform.position + Vector3.up * 0.5f;
            }
        }
    }
}
