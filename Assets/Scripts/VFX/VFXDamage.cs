using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VFXDamage : MonoBehaviour
{
    public CharacterStats characterStats;
    //���˱�ǩ�б�
    public List<string> enemyTagList;
    //���ι����ĵ����ܻ��б�
    private List<IHurt> enemyHurtList = new List<IHurt>();
    //����������
    public Collider hitCollider;
    //�����¼�
    private UnityAction<IHurt> onHitAction;
    public void Init()
    {
        hitCollider.enabled = false;
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
                //onHitAction.Invoke(enemy);
                #endregion 
                //other.GetComponent<CharacterStats>().TakeDamage(characterStats.skillConfig);
            }
            else if (enemy == null)
            {
                Debug.Log($"���ܻ�����{other.name}�������ܻ��ӿ�");
            }
        }
    }
}
