using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 宿主标记
/// </summary>
public interface IStateMachineOwner { }

public class StateMachine
{
    //当前状态
    private StateBase currentState;

    //是否包含当前状态
    public bool HasState
    {
        get => currentState != null;
    }

    //宿主
    private IStateMachineOwner owner;

    //状态字典
    private Dictionary<Type, StateBase> stateDic = new Dictionary<Type, StateBase>();


    public StateMachine(IStateMachineOwner owner)
    {
        Init(owner);
    }

    //初始化
    public void Init(IStateMachineOwner owner)
    {
        this.owner = owner;
    }

/// <summary>
/// 进入状态
/// </summary>
/// <typeparam name="T">状态类型</typeparam>
/// <param name="reLoadState">是否刷新状态</param>
   public void EnterState<T>(bool reLoadState = false)  where T : StateBase , new()
    {
        //如果状态一致,不切换
        if (HasState && currentState.GetType() == typeof(T) && !reLoadState)
            return;
        #region 结束当前状态
        if(HasState)
        {
            ExitCurrentState();
        }  
        #endregion


        #region 进入新状态
        currentState = LoadState<T>();
        EnterCurrentState();
        #endregion
    }

    /// <summary>
    /// 加载，创建新状态
    /// </summary>
    /// <typeparam name="T">状态类型</typeparam>
    /// <returns></returns>
    private StateBase LoadState<T>() where T : StateBase,new()
    {
        //获取状态实例
        Type stateType = typeof(T);
        if(!stateDic.TryGetValue(stateType,out StateBase state))
        {
            //创建状态实例并保存到字典中
            state = new T();
            state.Init(owner);
            stateDic.Add(stateType, state);
        }

        return state;
    }

    private void EnterCurrentState()
    {
        currentState.Enter();
        MonoManager.INSTANCE.AddUpdateAction(currentState.Update);
        MonoManager.INSTANCE.AddFixedUpdateAction(currentState.FixedUpdate);
        MonoManager.INSTANCE.AddLateUpdateAction(currentState.LateUpdate);
    }

    private void ExitCurrentState()
    {
        currentState.Exit();
        MonoManager.INSTANCE.RemoveUpdateAction(currentState.Update);
        MonoManager.INSTANCE.RemoveFixedUpdateAction(currentState.FixedUpdate);
        MonoManager.INSTANCE.RemoveLateUpdateAction(currentState.LateUpdate);
    }

    /// <summary>
    /// 停止运作，释放资源
    /// </summary>
    public void Clear()
    {
        ExitCurrentState();
        currentState = null;
        foreach (var item in stateDic.Values)
        {
            item.UnInit();
        }
        stateDic.Clear();
    }
}
