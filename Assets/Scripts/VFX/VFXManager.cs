using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VFXManager : SingleMonoBase<VFXManager>
{
    [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    [SerializeField, Header("特效播放倍率")] private float SpeedMult = 1;


    public void AddVFX(ParticleSystem particleSystem, float speedMult)
    {
        particleSystems.Add(particleSystem);
        foreach (var particle in particleSystems)
        {
            var main = particle.main;
            main.simulationSpeed = SpeedMult;
        }
    }

    public List<ParticleSystem> allParticleSystems => particleSystems;

    public void PauseVFX()
    {
        foreach (var particleSystem in allParticleSystems)
        {
            var main = particleSystem.main;
            main.simulationSpeed = 0f;
        }
    }
    public void SetVFXSpeed(float speedMult)
    {
        foreach (var particleSystem in allParticleSystems)
        {
            var main = particleSystem.main;
            main.simulationSpeed = speedMult;
        }
    }
    public void ResetVXF()
    {
        foreach (var particleSystem in allParticleSystems)
        {
            var main = particleSystem.main;
            main.simulationSpeed = SpeedMult;
        }
    }
}
