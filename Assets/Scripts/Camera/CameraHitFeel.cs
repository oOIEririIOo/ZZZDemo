using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CameraHitFeel : SingleMonoBase<CameraHitFeel>
{
    [SerializeField] private Animator currentCharacterAnimator;
    [SerializeField] private Animator currentEnemyAnimator;
    [SerializeField] private float slowMotionResetSpeed;
    [SerializeField] private Dictionary<CharacterNameList, Animator> characterAnimator = new Dictionary<CharacterNameList, Animator>();
    [SerializeField] private Dictionary<Transform, Animator> enemiesAnimator = new Dictionary<Transform, Animator>();
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;




   private Animator GetCurrentEnemyAnimator(EnemyTest enemy)
    {
        return enemy.anim;
    }
    private Animator GetCurrentCharacterAnimator()
    {
        return PlayerController.INSTANCE.playerModel.animator;
    }
}
