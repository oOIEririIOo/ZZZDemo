using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour,IHurt
{
    public Animator animator;
    private DamageDir damageTrans;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        //Debug.Log(gameObject.name + transform.rotation.eulerAngles.y);
    }
    public void HurtEvent(DamageDir dir, HitType hitType)
    {
        if( ( Mathf.Abs(PlayerController.INSTANCE.playerModel.ForwardAngle() - ForwardAngle()) ) <=60f)
        {
            damageTrans = DamageDir.Back;
        }
        else damageTrans = DamageDir.Front;
        Hurt(dir, hitType);
    }

    public void Hurt(DamageDir dir,HitType hitType)
    {
        //Debug.Log(hitType);
        //Debug.Log(dir);
        switch(damageTrans)
        {
            case DamageDir.Front:
                switch (hitType)
                {
                    case HitType.Light:
                        switch (dir)
                        {
                            case DamageDir.Left:
                                PlayAnimation("Hit_L_Front_Left");
                                break;
                            case DamageDir.Right:
                                PlayAnimation("Hit_L_Front_Right");
                                break;
                            case DamageDir.Up:
                                PlayAnimation("Hit_L_Front_Down");
                                break;
                            case DamageDir.Down:
                                PlayAnimation("Hit_L_Front_Up");
                                break;
                        }
                        break;
                    case HitType.Haven:
                        PlayAnimation("Hit_H_Front");
                        break;         
                    case HitType.Fly:
                        {

                        }
                        break;
                }
                break;
            case DamageDir.Back:
                switch (hitType)
                {
                    case HitType.Light:
                        switch (dir)
                        {
                            case DamageDir.Left:
                                PlayAnimation("Hit_L_Back_Left");
                                break;
                            case DamageDir.Right:
                                PlayAnimation("Hit_L_Back_Right");
                                break;
                            case DamageDir.Up:
                                PlayAnimation("Hit_L_Back_Down");
                                break;
                            case DamageDir.Down:
                                PlayAnimation("Hit_L_Back_Up");
                                break;
                        }
                        break;
                    case HitType.Haven:
                        PlayAnimation("Hit_H_Back");
                        break;
                    case HitType.Fly:
                        {

                        }
                        break;
                }
                break;
        }
    }

    public float ForwardAngle()
    {
        if (gameObject.transform.rotation.eulerAngles.y > 180)
        {
            return gameObject.transform.rotation.eulerAngles.y - 360;
        }
        else return gameObject.transform.rotation.eulerAngles.y;
    }

    public void PlayAnimation(string animationName, float fixedTransitionDuration = 0.05f)
    {
        animator.CrossFadeInFixedTime(animationName, fixedTransitionDuration);
    }
}
