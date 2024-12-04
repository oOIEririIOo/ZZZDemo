using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiVFX : MonoBehaviour
{
    public GameObject weaponBackPrefab;
    GameObject weaponEff;

    public Transform weaponBackSpawnPos;
    public void weaponBackPlay()
    {
        weaponEff = Instantiate(weaponBackPrefab, weaponBackSpawnPos);

    }

}
