using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnagiState : SwitchState
{

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
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiIdleState>(true);
                break;
            case PlayerState.Run:
            case PlayerState.Walk:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiRunState>(true);
                break;
            case PlayerState.RunEnd:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiRunEndState>();
                break;
            case PlayerState.TurnBack:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiTurnBackState>();
                break;
            case PlayerState.TurnBack_End:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiTurnBackEndState>();
                break;
            case PlayerState.Evade_Front:
            case PlayerState.Evade_Back:
                if (PlayerController.INSTANCE.evadeCnt == 2)
                {
                    return;
                }     
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiEvadeState>(true);
                PlayerController.INSTANCE.evadeCnt++;      
                break;
            case PlayerState.Evade_Front_End:
            case PlayerState.Evade_Back_End:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiEvadeEndState>();
                break;
            case PlayerState.NormalAttack:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiNormalAttackState>(true);
                break;
            case PlayerState.NormalAttackEnd:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiNormalAttackEndState>();
                break;
            case PlayerState.Attack_Rush:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiAttackRushState>();
                break;
            case PlayerState.Attack_Rush_End:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiAttackRushEndState>();
                break;
            case PlayerState.Counter:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiCounterState>();
                break;
            case PlayerState.Counter_End:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiCounterEndState>();
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
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiParryState>();
                break;
            case PlayerState.ParryEnd:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiParryEndState>();
                break;
            case PlayerState.Pause:
                PlayerController.INSTANCE.stateMachine.EnterState<PlayerPauseState>();
                break;
            case PlayerState.Hit:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiHitState>(true);
                break;

            //½ÇÉ«·ÖÖ§
            case PlayerState.Unagi_HavenAttack:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiHavenAttackState>();
                break;
            case PlayerState.Unagi_HavenAttackEnd:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiHavenAttackEndState>();
                break;
            case PlayerState.Unagi_BranchStart:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiBranchStartState>();
                break;
            case PlayerState.Branch:
            case PlayerState.SpBranch:
            case PlayerState.Unagi_HoldBranch:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiBranchState>();
                break;
            case PlayerState.Branch_End:
            case PlayerState.SpBranch_End:
            case PlayerState.Unagi_HoldBranchEnd:
                PlayerController.INSTANCE.stateMachine.EnterState<UnagiBranchEndState>();
                break;

        }     
    }
}
