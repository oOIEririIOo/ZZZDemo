using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class CameraHitFeel : SingleMonoBase<CameraHitFeel>
{
    [SerializeField] private Animator currentCharacterAnimator;
    [SerializeField] private Animator currentEnemyAnimator;
    [SerializeField] private List<Animator> allEnemyAnimator;
    public float slowMotionResetSpeed;
    [SerializeField] private Dictionary<CharacterNameList, Animator> characterAnimator = new Dictionary<CharacterNameList, Animator>();
    [SerializeField] private Dictionary<Transform, Animator> enemiesAnimator = new Dictionary<Transform, Animator>();
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    public Volume volume;

    Coroutine PauseFrameCoroutine;
    Coroutine SlowMotionCoroutine;
    Coroutine RemoveColorCoroutine;
    Coroutine QTEStartCoroutinue;
    Coroutine SwitichCharacterInQTECoroutinue;
    private ColorAdjustments colorAdjustments;


    //慢放
    public void SlowMotion(float time, float speedMult)
    {
        allEnemyAnimator = GetAllEnemyAnimator();
        currentCharacterAnimator = GetCurrentCharacterAnimator();
        if (currentCharacterAnimator == null || allEnemyAnimator == null)
        {
            Debug.LogWarning("Animator is null!");
            return;
        } 
        if (SlowMotionCoroutine != null)
        { StopCoroutine(SlowMotionCoroutine); }
        SlowMotionCoroutine = StartCoroutine(SlowMotionOnAnimation(time, speedMult));
        if (RemoveColorCoroutine != null)
        { StopCoroutine(RemoveColorCoroutine); }
        RemoveColorCoroutine = StartCoroutine(RemoveColor(time));
    }

    //QTE慢放
    public void QTEStart(float time,float speedMult)
    {
        QTEManager.INSTANCE.waitQTEInput = true;
        if (QTEManager.INSTANCE.canQTE == false) QTEManager.INSTANCE.canQTE = true;
        allEnemyAnimator = GetAllEnemyAnimator();
        currentCharacterAnimator = GetCurrentCharacterAnimator();
        PlayerController.INSTANCE.inputSystem.Player.Disable();//禁用玩家输入

        if (currentCharacterAnimator == null || allEnemyAnimator == null)
        {
            Debug.LogWarning("Animator is null!");
            return;
        }
        if (PauseFrameCoroutine != null)
        {
            StopCoroutine(PauseFrameCoroutine);
        }
        if (QTEStartCoroutinue != null)
        { StopCoroutine(QTEStartCoroutinue); }
        QTEStartCoroutinue = StartCoroutine(QTESlowMotion(time, speedMult));
    }

    public  void SwitichCharacterInQTE()
    {
        CameraManager.INSTANCE.virtualCameraComponent.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 3.2f;
        CameraManager.INSTANCE.virtualCameraComponent.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = 0.28f;
        
        if (QTEStartCoroutinue != null)
        { StopCoroutine(QTEStartCoroutinue); }
        
        float currentSpeed = 1f;
        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
        allEnemyAnimator = GetAllEnemyAnimator();
        currentCharacterAnimator = GetCurrentCharacterAnimator();
        if (SwitichCharacterInQTECoroutinue != null)
        {
            StopCoroutine(SwitichCharacterInQTECoroutinue);
        }
        SwitichCharacterInQTECoroutinue = StartCoroutine(SwitichCharacterInQTESlowMotion(1f, 0.5f));
    }

    //退出QTE
    public void CancelQTE()
    {
        PlayerController.INSTANCE.inputSystem.Player.Enable();//开启玩家输入
        PlayerController.INSTANCE.inputSystem.QTE.Disable();
        //QTEManager.INSTANCE.CancelQTE();
        CameraManager.INSTANCE.virtualCameraComponent.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 3.2f;
        CameraManager.INSTANCE.virtualCameraComponent.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.y = 0.28f;
        if (QTEStartCoroutinue != null)
        { StopCoroutine(QTEStartCoroutinue); }
        float currentSpeed = 1f;
        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
    }

    //钝帧
    public void PauseFrame(float time)
    {
        if (time == 0) {return; }
        currentCharacterAnimator = GetCurrentCharacterAnimator();
        if (PauseFrameCoroutine != null)
        {
            StopCoroutine(PauseFrameCoroutine);
        }
        PauseFrameCoroutine = StartCoroutine(PauseFrameOnAnimation(time));
    }


    //慢放协程
    IEnumerator SlowMotionOnAnimation(float time, float speedMult)
    {
        float currentSpeed = speedMult;
        
        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
        //float currentValue = -55f;
        yield return new WaitForSeconds(time);
        float minValue = 0.1f;
        while (Mathf.Abs(currentSpeed - 1) > minValue)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 1, Time.deltaTime * slowMotionResetSpeed);
            //currentValue = Mathf.Lerp(currentValue, 0, Time.deltaTime * 8f);
            //SetVolume(currentValue);
            currentCharacterAnimator.speed = currentSpeed;
            SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
            VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
            yield return null;

        }
        currentSpeed = 1;
        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
        //SetVolume(0f);
    }

    //屏幕失色效果协程
    IEnumerator RemoveColor(float time)
    {
        float targetValue = -55f;
        float currentValue = 0f;
        float minValue = 0.1f;
        while(Mathf.Abs(currentValue) < (Mathf.Abs(targetValue) - minValue))
        {
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * 8f);
            SetVolume(currentValue);
            yield return null;
        }
        yield return new WaitForSeconds(time*0.5f);
        while (Mathf.Abs(currentValue) > minValue)
        {
            currentValue = Mathf.Lerp(currentValue, 0, Time.deltaTime * 4.5f);
            SetVolume(currentValue);
            yield return null;
        }
        SetVolume(0f);
    }

    //QTE协程
    IEnumerator QTESlowMotion(float time, float speedMult)
    {
        float currentSpeed = speedMult;
        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
        yield return new WaitForSeconds(1f);//一秒后开始可以切人
        PlayerController.INSTANCE.inputSystem.QTE.Enable();
        Debug.Log("可以输入");
        yield return new WaitForSeconds(time-1f);
        float minValue = 0.1f;
        while (Mathf.Abs(currentSpeed - 1) > minValue)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 1, Time.deltaTime * slowMotionResetSpeed);
            
            currentCharacterAnimator.speed = currentSpeed;
            SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
            VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
            
            //Time.timeScale = currentSpeed;
            yield return null;

        }
        currentSpeed = 1;
        //Time.timeScale = currentSpeed;
        
        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
        QTEManager.INSTANCE.CancelQTE();
    }

    IEnumerator SwitichCharacterInQTESlowMotion(float time, float speedMult)
    {
        float currentSpeed = speedMult;
        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
        yield return new WaitForSeconds(time);
        float minValue = 0.1f;
        while (Mathf.Abs(currentSpeed - 1) > minValue)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 1, Time.deltaTime * slowMotionResetSpeed*3f);

            currentCharacterAnimator.speed = currentSpeed;
            SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
            VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);

            //Time.timeScale = currentSpeed;
            yield return null;

        }
        currentSpeed = 1;
        //Time.timeScale = currentSpeed;

        currentCharacterAnimator.speed = currentSpeed;
        SetAllEnemyAnimationSpeed(allEnemyAnimator, currentSpeed);
        VFXManager.INSTANCE.SetVFXSpeed(currentSpeed);
    }

    //钝帧协程
    IEnumerator PauseFrameOnAnimation(float time)
    {
        currentCharacterAnimator.speed = 0f;
        currentEnemyAnimator.speed = 0f;
        //VFXManager.INSTANCE.PauseVFX();
        yield return new WaitForSeconds(time);
        //VFXManager.INSTANCE.ResetVXF();
        currentCharacterAnimator.speed = 1f;
        currentEnemyAnimator.speed = 1f;
    }

    
    public void GetCurrentEnemyAnimation(EnemyController enemy)
    {
        currentEnemyAnimator = enemy.GetComponent<Animator>();
    }
    
    private List<Animator> GetAllEnemyAnimator()
    {
        List<EnemyController> enemies = AllEnemyController.INSTANCE.GetAllEnemy();
        if (enemies == null) return null;
        else
        {
            List<Animator> enemiesAnimators = new List<Animator>();
            foreach (var enemy in enemies)
            {
                enemiesAnimators.Add(enemy.GetComponent<Animator>());
            }
            return enemiesAnimators;
        }
    }
    private Animator GetCurrentCharacterAnimator()
    {
        return PlayerController.INSTANCE.playerModel.animator;
    }

    private void SetAllEnemyAnimationSpeed(List<Animator> enemyAnimators, float speedMult)
    {
        foreach (var animator in enemyAnimators)
        {
            animator.speed = speedMult;
        }
    }
    #region 震屏
    public void CameraShake(float shakeForce)
    {
        if (shakeForce == 0) { return; }
        cinemachineImpulseSource.GenerateImpulseWithForce(shakeForce);
    }
    #endregion
    private void SetVolume(float value)
    {
        //ClampedFloatParameter floatParameter = volume.GetComponent<ColorAdjustments>().saturation;
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments))
        {
            colorAdjustments.saturation.Override(value);
        }
        //volume.GetComponent<ColorAdjustments>().saturation.Override(0.2f);

        //volume.GetComponent<Vignette>().smoothness
    }
}
