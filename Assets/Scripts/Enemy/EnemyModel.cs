using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyModel : MonoBehaviour, IStateMachineOwner,IHurt
{
    //����������
    public Animator animator;
    //������Ϣ
    private AnimatorStateInfo stateInfo;
    //ת���ٶ�
    public float rotationSpeed = 8f;
    //Ѳ�ߵ�
    public List<Transform> locations;
    public Transform patrolRoute;

    public float patrolRange = 8f;

    //��ʼ����
    public Vector3 guardPos;

    public NavMeshAgent agent;
    public int locationIndex = 0;

    public Transform player;

    //�����¼�
    private Action<IHurt> onHitAction;

    //���˱�ǩ�б�
    public List<string> enemyTagList;
    //�����б�
    public WeaponController[] weapons;

    private void Awake()
    {
        player = null;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        guardPos = transform.position;
    }

    private void Start()
    {
        agent.updatePosition = false;
        Init(enemyTagList);
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.transform;
        }    
    }
    private void OnHit(IHurt enemy)
    {
        Debug.Log(((Component)enemy).name);
    }

    /// <summary>
    /// �����˺����
    /// </summary>
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
    }
    /// <summary>
    /// �ر��˺����
    /// </summary>
    public void StopHit(int weaponIndex)
    {
        weapons[weaponIndex].StopHit();
    }


    public void Init(List<string> enemyTagList)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].Init(enemyTagList, OnHit);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }
}
