using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Events;
using UnityEngine.Events;

/// <summary>
/// ����������
/// </summary>
public class WeaponController : MonoBehaviour
{
    public CharacterStats characterStats;
    //private int currentAttackIndex;
    //���˱�ǩ�б�
    private List<string> enemyTagList;
    //���ι����ĵ����ܻ��б�
    private List<IHurt> enemyHurtList = new List<IHurt>();
    //����������
    public Collider hitCollider;
    //�����¼�
    private UnityAction<IHurt> onHitAction;
    public UnityEvent<CharacterStats> onTakeDemage;
    public int hitIndex;

    public void Init(List<string> enemyTagList, UnityAction<IHurt> onHitAction)
    {
        hitCollider.enabled = false;
        this.enemyTagList = enemyTagList;
        this.onHitAction = onHitAction;
    }

    /// <summary>
    /// �����˺����
    /// </summary>
    public void StartHit()
    {
        hitCollider.enabled = true;
    }
    /// <summary>
    /// �ر��˺����
    /// </summary>
    public void StopHit()
    {
        hitCollider.enabled = false;
        //����ܻ��б�
        enemyHurtList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        //�����ײĿ���ڵ��˱�ǩ��
        if (enemyTagList.Contains(other.tag))
        {
            IHurt enemy = other.GetComponent<IHurt>();
            if (enemy != null && !enemyHurtList.Contains(enemy))
            {
                
                //��¼�������ĵ���
                enemyHurtList.Add(enemy);
                #region �ܻ�
                //���������¼����ϼ������ܻ���
                onHitAction.Invoke(enemy);
                #endregion 
                //other.GetComponent<CharacterStats>().TakeDamage(characterStats.skillConfig); 
                /*
                if(other.TryGetComponent<EnemyTest>(out EnemyTest enemyTest))
                {
                    enemyTest.HurtEvent(PlayerController.INSTANCE.playerModel.characterStats.skillConfig.currentAttackInfo.damageDir,
                        PlayerController.INSTANCE.playerModel.characterStats.skillConfig.currentAttackInfo.hitType);
                }
                /*
                Vector3 location = this.transform.position;
                Vector3 closestPoint = other.ClosestPoint(location);//��ȡ��ײλ��
                Vector3 forword = characterStats.gameObject.transform.forward;
                VFXPoolManager.INSTANCE.SpawnHitVfx(other.GetComponent<CharacterStats>().characterName, PlayerController.INSTANCE.playerModel.characterStats.skillConfig.currentAttackInfo, closestPoint,forword);
                */
            }
            else if (enemy == null)
            {
                Debug.Log($"���ܻ�����{other.name}�������ܻ��ӿ�");
            }
        }
    }




}
