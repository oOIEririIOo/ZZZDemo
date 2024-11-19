using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式改造器
/// </summary>
/// <typeparam name="T">子类</typeparam>
public class SingleMonoBase <T>: MonoBehaviour where T :SingleMonoBase <T>
{
    //子类的单例
    public static T INSTANCE;

    protected private virtual void Awake()
    {
        //DontDestroyOnLoad(this);
        if (INSTANCE != null)
        {
            Debug.LogError(this + "不符合单例模式");
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
    /// 清除子类单例
    /// </summary>
    public void Destroy()
    {
        INSTANCE = null;
    }
}
