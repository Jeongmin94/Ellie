using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public enum ParticleType
    {
        CastFireball,
        MeleeAttack,
        MeleeAttackHit,
        WeaponAttackSwing,
        WeaponAttackhit,
    }

    [SerializeField] private MonsterEffectData data;
    private Dictionary<ParticleType, ParticleSystem> particles;

    private void Awake()
    {
        particles = new();
    }

    public ParticleSystem GetParticle(ParticleType type)
    {
        ParticleSystem particle;
        if(particles.TryGetValue(type, out particle))
        {
            return particle;
        }
        else
        {
            particles.Add(type, data.GetParticle(type));
            if(particles.TryGetValue(type, out particle))
            {
                return particle;
            }
        }

        return null;
    }

}
