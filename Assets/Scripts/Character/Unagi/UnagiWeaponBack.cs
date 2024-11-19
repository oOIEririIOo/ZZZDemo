using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiWeaponBack : MonoBehaviour
{
    ParticleSystem weaponBackParticle;
    private void OnEnable()
    {
        weaponBackParticle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!weaponBackParticle.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
