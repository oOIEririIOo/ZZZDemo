using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttacks_Manager : MonoBehaviour
{
    public float delay;
    private float reinitializeDelay;
    float delayShootProjectile;
    bool isCasting;

    public GameObject slashManager;

    public Transform spawnOffSet;
    public Transform target;

    public GameObject[] FXList_Cast;
    public GameObject[] FXList_Hit;
    public GameObject[] FXList_Projectile;
    int currentFX_Element;
    int nextFX_Element;

    void Awake()
    {
        reinitializeDelay = delay;
        delay = 1;

        if (slashManager != null)
        {
            slashManager.SetActive(false);
        }

    }

    void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }

        if(delay <= 0)
        {
            currentFX_Element = nextFX_Element;
            CastProjectile();
            delay = reinitializeDelay;
        }
        
        if(isCasting)
        {
            ShootProjectile();
        }

        if (slashManager != null)
        {
            ChangeEffect();
        }

        InputsFXElement();

    }

    void ChangeEffect()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            slashManager.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    void InputsFXElement()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (nextFX_Element < FXList_Cast.Length -1)
            {
                nextFX_Element += 1;
            }

            else if (nextFX_Element >= FXList_Cast.Length -1)
            {
                nextFX_Element = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (nextFX_Element > 0)
            {
                nextFX_Element -= 1;
            }

            else if (nextFX_Element <= 0)
            {
                nextFX_Element = FXList_Cast.Length -1;
            }

        }
    }

    void CastProjectile()
    {
        Instantiate(FXList_Cast[currentFX_Element], spawnOffSet.position, Quaternion.identity);
        delayShootProjectile = 0.7f;

        isCasting = true;

    }

    void ShootProjectile()
    {
        delayShootProjectile -= Time.deltaTime;

        if (delayShootProjectile <= 0)
        {
            GameObject projectileInstTransform;
            projectileInstTransform = Instantiate(FXList_Projectile[currentFX_Element], spawnOffSet.position, Quaternion.identity);

            Vector3 projectileDir = (target.position - spawnOffSet.position).normalized;
            projectileInstTransform.GetComponent<MagicAttacks_Projectile>().Setup(projectileDir);
            delayShootProjectile = 0;

            projectileInstTransform.GetComponent<MagicAttacks_Projectile>().FX_Hit = FXList_Hit[currentFX_Element];


            isCasting = false;
            Destroy(projectileInstTransform.gameObject, 4f);

        }
    }

  
}
