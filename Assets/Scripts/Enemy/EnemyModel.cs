using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyModel : MonoBehaviour, IStateMachineOwner,IHurt
{
    //动画控制器
    public Animator animator;
    //动画信息
    private AnimatorStateInfo stateInfo;
    //转向速度
    public float rotationSpeed = 8f;
    //巡逻点
    public List<Transform> locations;
    public Transform patrolRoute;

    public float patrolRange = 8f;

    //初始坐标
    public Vector3 guardPos;

    public NavMeshAgent agent;
    public int locationIndex = 0;

    public Transform player;

    //命中事件
    private Action<IHurt> onHitAction;

    //敌人标签列表
    public List<string> enemyTagList;
    //武器列表
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
    /// 开启伤害检测
    /// </summary>
    public void StartHit(int weaponIndex)
    {
        weapons[weaponIndex].StartHit();
    }
    /// <summary>
    /// 关闭伤害检测
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
