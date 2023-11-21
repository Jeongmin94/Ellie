using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterParticleType
{
    MeleeAttack,
    MeleeHit,
    WeaponAttack,
    WeaponHit,
    ProjectileCast,
    ProjectileHit,
}
public class MonsterParticleController : MonoBehaviour
{

    [SerializeField] private MonsterParticleData data;
    private Dictionary<MonsterParticleType, ParticleSystem> particles;

    private void Awake()
    {
        particles = new();
    }

    public ParticleSystem GetParticle(MonsterParticleType type)
    {
        ParticleSystem particle;
        if (particles.TryGetValue(type, out particle))
        {
            particle.transform.position = gameObject.transform.position + data.GetParticleOffset(type);
            return particle;
        }
        else
        {
            ParticleSystem particleObj = Instantiate(data.GetParticleSystem(type),transform);
            particles.Add(type, particleObj);
            if (particles.TryGetValue(type, out particle))
            {
                particle.transform.position = gameObject.transform.position + data.GetParticleOffset(type);
                return particle;
            }
        }
        return null;
    }

    public bool PlayParticle(MonsterParticleType type)
    {
        ParticleSystem particle = GetParticle(type);
        if (particle == null) return false;
        particle.Play();
        return true;
    }
}
