using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IStateMachineOwner, IHurt,ISwitichScene
{
    public EnemyModel enemyModel;
    //当前状态
    public EnemyState currentState;
    //状态机
    private StateMachine stateMachine;
    //属性
    public CharacterStats characterStats;

    public bool isDead = false;


    protected void Awake()
    {
        stateMachine = new StateMachine(this);
        characterStats = GetComponent<CharacterStats>();
    }
    private void Start()
    {
        SwitchState(EnemyState.Idle);
        characterStats.CurrentHealth = characterStats.MaxHealth;
        characterStats.CurrentDefence = characterStats.BaseDefence;
        GameManager.INSTANCE.AddObserver(this);
    }

    private void OnEnable()
    {
        //SwitchState(EnemyState.Idle);
    }

    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.INSTANCE.RemoveObserver(this);
        //stateMachine.Clear();

        if(QuestManager.IsInitialized && isDead)
        {
            QuestManager.INSTANCE.UpdateQuestProgress(this.name, 1);
        }
    }

    private void Update()
    {
        if (characterStats.CurrentHealth == 0)
        {
            SwitchState(EnemyState.Dead);
        }
    }

    public void SwitchState(EnemyState enemyState)
    {
        currentState = enemyState;
        switch (enemyState)
        {
            case EnemyState.Idle:
                stateMachine.EnterState<EnemyIdleState>();
                break;
            case EnemyState.Patrol:
                stateMachine.EnterState<EnemyPatrolState>();
                break;
            case EnemyState.Turn:
                stateMachine.EnterState<EnemyTurnState>();
                break;
            case EnemyState.Chase:
                stateMachine.EnterState<EnemyChaseState>();
                break;
            case EnemyState.React:
                stateMachine.EnterState<EnemyReactState>();
                break;
            case EnemyState.Attack:
                stateMachine.EnterState<EnemyAttackState>();
                break;
            case EnemyState.Dead:
                stateMachine.EnterState<EnemyDeadState>();
                break;

        }
    }
    public void PlayAnimation(string animationName, float fixedTransitionDuration = 0.25f)
    {
        enemyModel.animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration);
    }

    public void LoadScene()
    {
        stateMachine.Clear();
    }

    public void DestroyModel()
    {
        stateMachine.Clear();
        Destroy(gameObject.GetComponentInParent<Transform>().gameObject, 1f);
        //Destroy(gameObject.transform.parent, 1f);
    }

    public void OnSwitchScene()
    {
        stateMachine.Clear();
    }
}
