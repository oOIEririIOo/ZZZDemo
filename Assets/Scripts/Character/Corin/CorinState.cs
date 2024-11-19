using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorinState : SwitchState
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
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerIdleState>(true);
                    break;
                case PlayerState.Run:
                case PlayerState.Walk:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerRunState>(true);
                    break;
                case PlayerState.RunEnd:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerRunEndState>();
                    break;
                case PlayerState.TurnBack:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerTurnBackState>();
                    break;
                case PlayerState.TurnBack_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerTurnBackEndState>();
                    break;
                case PlayerState.Evade_Front:
                case PlayerState.Evade_Back:
                    if (PlayerController.INSTANCE.evadeTimer != 1f) return;
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerEvadeState>(true);
                PlayerController.INSTANCE.evadeCnt++;
                    if (PlayerController.INSTANCE.evadeCnt == 2)
                    PlayerController.INSTANCE.evadeTimer -= 1f;
                    break;
                case PlayerState.Evade_Front_End:
                case PlayerState.Evade_Back_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerEvadeEndState>();
                    break;
                case PlayerState.NormalAttack:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerNormalAttackState>(true);
                    break;
                case PlayerState.NormalAttackEnd:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerNormalAttackEndState>();
                    break;
                case PlayerState.Attack_Rush:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerAttackRushState>();
                    break;
                case PlayerState.Attack_Rush_End:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerAttackRushEndState>();
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
                case PlayerState.Pause:
                    PlayerController.INSTANCE.stateMachine.EnterState<PlayerPauseState>();
                    break;
            }
        

    }
}
