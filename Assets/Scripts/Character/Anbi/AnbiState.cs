using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnbiState : SwitchState
{

    public bool perfectTiming;
    public override void Awake()
    {
        base.Awake();
    }
    public override void SwitichCharacterState(PlayerState playerState)
    {
        base.SwitichCharacterState(playerState);
        playerModel.currentState = playerState;
        CharacterNameList characterName = characterStats.characterName;
       
            switch (playerState)
            {
                case PlayerState.Idle:
                case PlayerState.Idle_AFK:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiIdleState>(true);
                    break;
                case PlayerState.Run:
                case PlayerState.Walk:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiRunState>(true);
                    break;
                case PlayerState.RunEnd:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiRunEndState>();
                    break;
                case PlayerState.TurnBack:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiTurnBackState>();
                    break;
                case PlayerState.TurnBack_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiTurnBackEndState>();
                    break;
                case PlayerState.Evade_Front:
                case PlayerState.Evade_Back:
                if (PlayerController.INSTANCE.evadeCnt == 2) return;
                PlayerController.INSTANCE.stateMachine.EnterState<AnbiEvadeState>(true);
                PlayerController.INSTANCE.evadeCnt++;
                break;
                case PlayerState.Evade_Front_End:
                case PlayerState.Evade_Back_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiEvadeEndState>();
                    break;
                case PlayerState.NormalAttack:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiNormalAttackState>(true);
                    break;
                case PlayerState.NormalAttackEnd:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiNormalAttackEndState>();
                    break;
                case PlayerState.Anbi_PerfectAttack:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiPerfectAttackState>();
                    break;
                case PlayerState.Anbi_PerfectAttack_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiPerfectAttackEndState>();
                    break;
                case PlayerState.Branch:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiBranchState>();
                    break;
                case PlayerState.Anbi_PerfectBranch:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiPerfectBranchState>();
                    break;
                case PlayerState.Branch_End:
                case PlayerState.SpBranch_End:
                case PlayerState.Anbi_PerfectBranch_End:
                case PlayerState.Anbi_PerfectSPBranch_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiBranchEndState>();
                    break;
                case PlayerState.Attack_Rush:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiAttackRushState>();
                    break;
                case PlayerState.Attack_Rush_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiAttackRushEndState>();
                    break;
                case PlayerState.Counter:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiAttackCounterState>();
                    break;
                case PlayerState.Counter_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiAttackCounterEndState>();
                    break;
                case PlayerState.BigSkillStart:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerBigSkillStartState>();
                    break;
                case PlayerState.BigSkill:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerBigSkillState>();
                    break;
                case PlayerState.BigSkillEnd:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerBigSkillEndState>();
                    break;
                case PlayerState.SwitchInNormal:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerSwitchInNoramlState>();
                    break;
                case PlayerState.Parry:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiParryState>();
                    break;
                case PlayerState.ParryEnd:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiParryEndState>();
                    break;
                case PlayerState.Hit:
                    PlayerController.INSTANCE.stateMachine.EnterState<AnbiHitState>(true);
                    break;
                case PlayerState.Pause:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerPauseState>();
                    break;
            case PlayerState.QTE:
                PlayerController.INSTANCE.stateMachine.EnterState<AnbiQTEState>();
                break;
            case PlayerState.QTE_End:
                PlayerController.INSTANCE.stateMachine.EnterState<AnbiQTEEndState>();
                break;
            default:
                    Debug.Log("无法找到该状态：" + playerState);
                    break;
            }
             
    }
    public void PerfectTimingStart()
    {
        perfectTiming = true;
    }

    public void PerfectTimingEnd()
    {
        perfectTiming = false;
    }
}
