using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VFXDamage : MonoBehaviour
{
    public CharacterStats characterStats;
    //敌人标签列表
    public List<string> enemyTagList;
    //单次攻击的敌人受击列表
    private List<IHurt> enemyHurtList = new List<IHurt>();
    //武器触发器
    public Collider hitCollider;
    //命中事件
    private UnityAction<IHurt> onHitAction;
    public void Init()
    {
        hitCollider.enabled = false;
    }

    /// <summary>
    /// 开启伤害检测
    /// </summary>
    public void StartHit()
    {
        hitCollider.enabled = true;
    }
    /// <summary>
    /// 关闭伤害检测
    /// </summary>
    public void StopHit()
    {
        hitCollider.enabled = false;
        //清空受击列表
        enemyHurtList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        //检测碰撞目标在敌人标签里
        if (enemyTagList.Contains(other.tag))
        {
            IHurt enemy = other.GetComponent<IHurt>();
            if (enemy != null && !enemyHurtList.Contains(enemy))
            {
                //记录攻击到的敌人
                enemyHurtList.Add(enemy);
                #region 受击
                //触发命中事件（上级处理受击）
                //onHitAction.Invoke(enemy);
                #endregion 
                //other.GetComponent<CharacterStats>().TakeDamage(characterStats.skillConfig);
            }
            else if (enemy == null)
            {
                Debug.Log($"该受击对象{other.name}不包含受击接口");
            }
        }
    }
}
