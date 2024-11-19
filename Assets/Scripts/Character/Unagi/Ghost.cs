using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ghost : MonoBehaviour
{
    public Transform target;
    public Transform ghost;
    private Tweener m_tweener;
    private void OnEnable()
    {
        m_tweener = ghost.DOMove(target.position, 0.5f).OnUpdate(() =>
        {
            m_tweener.ChangeEndValue(target.position, true);
        });
    }

    
}
