using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : SingleMonoBase<PlayerController>, IStateMachineOwner
{
    //输入系统
    public InputSystem inputSystem;

    //玩家移动输入
    public Vector2 inputMoveVec2;

    public PlayerModel playerModel;

    public StateMachine stateMachine;

    //闪避计时器
    public float evadeTimer = 0.8f;

    //闪避计数器
    public int evadeCnt = 0;

    //转向速度
    public float rotationSpeed = 8f;

    //玩家配置信息
    public PlayerConfig playerConfig;

    //配队
    [HideInInspector]public List<PlayerModel> controllableModels;

    //当前角色编号
    public int currentModelIndex;

    public Dictionary<CharacterNameList, int> characterDic;

    //敌人标签列表
    public List<string> enemyTagList;

    //命中事件
    private Action<IHurt> onHitAction;

    //角色信息
    public List<GameObject> characterInfo;

    public GameObject[] vfxPos;

    public bool mouseOpen = false;
    public bool mousePressed = false;
    public bool branchPressed = false;
    public bool branchTap = false;
    public bool branchHold = false;
    public bool isDodge;

    protected private override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        stateMachine = new StateMachine(this);
        inputSystem = new InputSystem();
        controllableModels = new List<PlayerModel>();
        characterDic = new Dictionary<CharacterNameList, int>();

        #region 生成角色模型
        for (int i = 0;i<playerConfig.models.Length;i++)
        {
            GameObject model = Instantiate(playerConfig.models[i], transform);
            controllableModels.Add(model.GetComponent<PlayerModel>());           
            controllableModels[i].gameObject.SetActive(false);
            //初始化角色模型
            controllableModels[i].Init(enemyTagList);
            characterInfo.Add(controllableModels[i].gameObject);
            
        }
        #endregion

        #region 操控配队中第一个角色
        controllableModels[0].gameObject.SetActive(true);
        currentModelIndex = 0;
        playerModel = controllableModels[currentModelIndex];
        #endregion

        #region 鼠标按键逻辑
        inputSystem.Player.Attack2.started += ctx =>
        {
            
            mousePressed = false;
        };
        inputSystem.Player.Attack2.performed += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                
                mousePressed = true;
            }

        };
        inputSystem.Player.Attack2.canceled += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                
                mousePressed = false;
            }
        };
        #endregion
        /*
        #region 技能分支按键逻辑
        inputSystem.Player.Branch2.started += ctx =>
        {
            Debug.Log("操作开始");
            branchPressed = false;
        };
        inputSystem.Player.Branch2.performed += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                Debug.Log("执行长按逻辑");
                branchPressed = true;
            }

        };
        inputSystem.Player.Branch2.canceled += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                Debug.Log("执行取消逻辑");
                if(branchPressed)
                {
                    branchHold = true;
                    branchTap = false;
                }
                else
                {
                    branchHold = false;
                    branchTap = true;
                }
                branchPressed = false;
                
            }
        };
        #endregion
        */
        for (int i = 0; i < playerConfig.models.Length; i++)
        {
            characterDic.Add(playerConfig.models[i].GetComponent<CharacterStats>().characterName, i);
        }
    }
    private void Start()
    {
        //LockMouse();
        SwitchState(PlayerState.Idle);
    }


    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="playerState">状态</param>
    public void SwitchState(PlayerState playerState)
    {
        playerModel.switchState.SwitichCharacterState(playerState);
        /*
        playerModel.currentState = playerState;
        CharacterNameList characterName = playerModel.characterName;
        if( ! CheckSpState(playerState))
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                case PlayerState.Idle_AFK:
                    stateMachine.EnterState<PlayerIdleState>(true);
                    break;
                case PlayerState.Run:
                case PlayerState.Walk:
                    stateMachine.EnterState<PlayerRunState>(true);
                    break;
                case PlayerState.RunEnd:
                    stateMachine.EnterState<PlayerRunEndState>();
                    break;
                case PlayerState.TurnBack:
                    stateMachine.EnterState<PlayerTurnBackState>();
                    break;
                case PlayerState.TurnBack_End:
                    stateMachine.EnterState<PlayerTurnBackEndState>();
                    break;
                case PlayerState.Evade_Front:
                case PlayerState.Evade_Back:
                    if (evadeTimer != 1f) return;
                    stateMachine.EnterState<PlayerEvadeState>(true);
                    evadeCnt++;
                    if (evadeCnt == 2)
                        evadeTimer -= 1f;
                    break;
                case PlayerState.Evade_Front_End:
                case PlayerState.Evade_Back_End:
                    stateMachine.EnterState<PlayerEvadeEndState>();
                    break;
                case PlayerState.NormalAttack:
                    stateMachine.EnterState<PlayerNormalAttackState>(true);
                    break;
                case PlayerState.NormalAttackEnd:
                    stateMachine.EnterState<PlayerNormalAttackEndState>();
                    break;
                case PlayerState.Attack_Rush:
                    stateMachine.EnterState<PlayerAttackRushState>();
                    break;
                case PlayerState.Attack_Rush_End:
                    stateMachine.EnterState<PlayerAttackRushEndState>();
                    break;
                case PlayerState.BigSkillStart:
                    stateMachine.EnterState<PlayerBigSkillStartState>();
                    break;
                case PlayerState.BigSkill:
                    stateMachine.EnterState<PlayerBigSkillState>();
                    break;
                case PlayerState.BigSkillEnd:
                    stateMachine.EnterState<PlayerBigSkillEndState>();
                    break;
                case PlayerState.SwitchInNormal:
                    stateMachine.EnterState<PlayerSwitchInNoramlState>();
                    break;
                case PlayerState.Pause:
                    stateMachine.EnterState<PlayerPauseState>();
                    break;

                //角色分支
                case PlayerState.Unagi_HavenAttack:
                    stateMachine.EnterState<UnagiHavenAttackState>();
                    break;
                case PlayerState.Unagi_HavenAttackEnd:
                    stateMachine.EnterState<UnagiHavenAttackEndState>();
                    break;
            }     
        }
        //特殊状态
        else
        {
            switch(characterName)
            {
                //选择角色类型
                case CharacterNameList.Unagi:
                    //选择角色状态
                    switch(playerState)
                    {
                        case PlayerState.NormalAttack:
                            stateMachine.EnterState<UnagiNormalAttackState>(true);
                            break;
                        case PlayerState.Branch:
                            stateMachine.EnterState<UnagiBranchState>();
                            break;
                    }
                    break;
                


            }
        }
        */
    }

    /// <summary>
    /// 切换角色
    /// </summary>
    /// <param name="isSPExit">是否为特殊退出</param>
    public void SwitchNextModel(bool isSPExit)
    {
        PlayerController.INSTANCE.playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
        //刷新状态机
        stateMachine.Clear();
        //退出当前模型
        if (isSPExit)
        {
            playerModel.ExitSpecial();
        }
        else
        {
            playerModel.ExitNormal();
        }
        
        
        #region 控制下一个模型
        currentModelIndex++;
            if(currentModelIndex >= controllableModels.Count)
            {
                currentModelIndex = 0;
            }
        PlayerModel nextmodel = controllableModels[currentModelIndex];
        nextmodel.gameObject.SetActive(true);
        Vector3 prevPos = playerModel.transform.position;
        Quaternion prevRot = playerModel.transform.rotation;
        playerModel = nextmodel;
        #endregion
        //进入下一个模型
        playerModel.Enter(prevPos, prevRot);
        //切换到入场状态
        SwitchState(PlayerState.SwitchInNormal);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    /// <param name="fixedTransitionDuration">过渡时间</param>
    public void PlayAnimation(string animationName,float fixedTransitionDuration = 0.25f)
    {
        playerModel.animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration);
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    /// <param name="fixedTransitionDuration">过渡时间</param>
    /// <param name="fixedTimeOffset">动画起始播放偏移</param>
    public void PlayAnimation(string animationName, float fixedTransitionDuration,float fixedTimeOffset)
    {
        playerModel.animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration,0,fixedTimeOffset);
    }

    private void Update()
    {
        //更新玩家移动输入
        inputMoveVec2 = inputSystem.Player.Move.ReadValue<Vector2>().normalized;

        //LockMouse();

        if(inputSystem.Player.Mouse.triggered)
        {
            mouseOpen = !mouseOpen;
            if(!mouseOpen)
            {
                //锁定光标在中间
                Cursor.lockState = CursorLockMode.Locked;
                //隐藏光标
                Cursor.visible = false;
                //相机不移动
                CameraManager.INSTANCE.virtualCameraComponent.enabled = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                CameraManager.INSTANCE.virtualCameraComponent.enabled = false;
            }
        }

        if(inputSystem.Player.Menu.triggered && SceneManager.GetActiveScene().name != "Main")
        {
            SceneController.INSTANCE.TransitionToMain();
        }

        //恢复闪避计时器
        /*
        if(evadeTimer < 0.8f && evadeCnt == 2)
        {
            evadeTimer += Time.deltaTime;
            if(evadeTimer >0.8f)
            {
                evadeTimer = 0.8f;
                evadeCnt = 0;
            }
        }
        */
        if(evadeCnt != 0)
        {
            evadeTimer += Time.deltaTime;
            if (evadeTimer > 0.8f)
            {
                evadeTimer = 0.8f;
                evadeCnt = 0;
            }
        }
        
    }
    /*
    private void LockMouse()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            //锁定光标在中间
            Cursor.lockState = CursorLockMode.Locked;
            //隐藏光标
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
    */
    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
    
    /*
    public bool CheckSpState(PlayerState state)
    {
        for(int i = 0;i<playerModel.spStates.Count;i++)
        {
            if (playerModel.spStates[i] == state)
            {
                return true;
            }
        }
        return false;
    }
    */
}

