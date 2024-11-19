using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģʽ������
/// </summary>
/// <typeparam name="T">����</typeparam>
public class SingleMonoBase <T>: MonoBehaviour where T :SingleMonoBase <T>
{
    //����ĵ���
    public static T INSTANCE;

    protected private virtual void Awake()
    {
        //DontDestroyOnLoad(this);
        if (INSTANCE != null)
        {
            Debug.LogError(this + "�����ϵ���ģʽ");
            Destroy(this.gameObject);
        }
        else
        {
            INSTANCE = (T)this;
            
        }

    }

    public static bool IsInitialized
    {
        get { return INSTANCE != null; }
    }

    private void OnDestroy()
    {
        Destroy();
    }

    /// <summary>
    /// ������൥��
    /// </summary>
    public void Destroy()
    {
        INSTANCE = null;
    }
}
