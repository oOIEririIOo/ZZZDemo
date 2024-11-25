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
/// ��ҿ�����
/// </summary>
public class PlayerController : SingleMonoBase<PlayerController>, IStateMachineOwner
{
    //����ϵͳ
    public InputSystem inputSystem;

    //����ƶ�����
    public Vector2 inputMoveVec2;

    public PlayerModel playerModel;

    public StateMachine stateMachine;

    //���ܼ�ʱ��
    public float evadeTimer = 0.8f;

    //���ܼ�����
    public int evadeCnt = 0;

    //ת���ٶ�
    public float rotationSpeed = 8f;

    //���������Ϣ
    public PlayerConfig playerConfig;

    //���
    [HideInInspector]public List<PlayerModel> controllableModels;

    //��ǰ��ɫ���
    public int currentModelIndex;

    public Dictionary<CharacterNameList, int> characterDic;

    //���˱�ǩ�б�
    public List<string> enemyTagList;

    //�����¼�
    private Action<IHurt> onHitAction;

    //��ɫ��Ϣ
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

        #region ���ɽ�ɫģ��
        for (int i = 0;i<playerConfig.models.Length;i++)
        {
            GameObject model = Instantiate(playerConfig.models[i], transform);
            controllableModels.Add(model.GetComponent<PlayerModel>());           
            controllableModels[i].gameObject.SetActive(false);
            //��ʼ����ɫģ��
            controllableModels[i].Init(enemyTagList);
            characterInfo.Add(controllableModels[i].gameObject);
            
        }
        #endregion

        #region �ٿ�����е�һ����ɫ
        controllableModels[0].gameObject.SetActive(true);
        currentModelIndex = 0;
        playerModel = controllableModels[currentModelIndex];
        #endregion

        #region ��갴���߼�
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
        #region ���ܷ�֧�����߼�
        inputSystem.Player.Branch2.started += ctx =>
        {
            Debug.Log("������ʼ");
            branchPressed = false;
        };
        inputSystem.Player.Branch2.performed += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                Debug.Log("ִ�г����߼�");
                branchPressed = true;
            }

        };
        inputSystem.Player.Branch2.canceled += ctx =>
        {
            if (ctx.interaction is HoldInteraction)
            {
                Debug.Log("ִ��ȡ���߼�");
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
    /// �л�״̬
    /// </summary>
    /// <param name="playerState">״̬</param>
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

                //��ɫ��֧
                case PlayerState.Unagi_HavenAttack:
                    stateMachine.EnterState<UnagiHavenAttackState>();
                    break;
                case PlayerState.Unagi_HavenAttackEnd:
                    stateMachine.EnterState<UnagiHavenAttackEndState>();
                    break;
            }     
        }
        //����״̬
        else
        {
            switch(characterName)
            {
                //ѡ���ɫ����
                case CharacterNameList.Unagi:
                    //ѡ���ɫ״̬
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
    /// �л���ɫ
    /// </summary>
    /// <param name="isSPExit">�Ƿ�Ϊ�����˳�</param>
    public void SwitchNextModel(bool isSPExit)
    {
        PlayerController.INSTANCE.playerModel.characterStats.skillConfig.currentNormalAttackIndex = 1;
        //ˢ��״̬��
        stateMachine.Clear();
        //�˳���ǰģ��
        if (isSPExit)
        {
            playerModel.ExitSpecial();
        }
        else
        {
            playerModel.ExitNormal();
        }
        
        
        #region ������һ��ģ��
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
        //������һ��ģ��
        playerModel.Enter(prevPos, prevRot);
        //�л����볡״̬
        SwitchState(PlayerState.SwitchInNormal);
    }

    /// <summary>
    /// ���Ŷ���
    /// </summary>
    /// <param name="animationName">��������</param>
    /// <param name="fixedTransitionDuration">����ʱ��</param>
    public void PlayAnimation(string animationName,float fixedTransitionDuration = 0.25f)
    {
        playerModel.animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration);
    }
    /// <summary>
    /// ���Ŷ���
    /// </summary>
    /// <param name="animationName">��������</param>
    /// <param name="fixedTransitionDuration">����ʱ��</param>
    /// <param name="fixedTimeOffset">������ʼ����ƫ��</param>
    public void PlayAnimation(string animationName, float fixedTransitionDuration,float fixedTimeOffset)
    {
        playerModel.animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration,0,fixedTimeOffset);
    }

    private void Update()
    {
        //��������ƶ�����
        inputMoveVec2 = inputSystem.Player.Move.ReadValue<Vector2>().normalized;

        //LockMouse();

        if(inputSystem.Player.Mouse.triggered)
        {
            mouseOpen = !mouseOpen;
            if(!mouseOpen)
            {
                //����������м�
                Cursor.lockState = CursorLockMode.Locked;
                //���ع��
                Cursor.visible = false;
                //������ƶ�
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

        //�ָ����ܼ�ʱ��
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
            //����������м�
            Cursor.lockState = CursorLockMode.Locked;
            //���ع��
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

