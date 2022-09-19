using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : GenerticSingleton<PoolsManager>
{
    public Transform myTransform;
    [Header("Partical")]
    [SerializeField] private Transform parentPartic;

    [SerializeField] private List<ParticleSystem> particleSystem_ConfettiExplosions;
    public override void Awake()
    {
        base.Awake();
        if (myTransform == null) { myTransform = this.transform; }
    }
    public ParticleSystem GetParticleSystem_ConfettiExplosion_Random()
    {
        return particleSystem_ConfettiExplosions[Random.Range(0, particleSystem_ConfettiExplosions.Count)];
    }
}
