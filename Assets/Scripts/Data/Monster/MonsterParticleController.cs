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
    Hit,
    HeadShot
}

public class MonsterParticleController : MonoBehaviour
{
    [SerializeField] private MonsterParticleData data;
    private ParticleSystem particle;
    private Dictionary<MonsterParticleType, ParticleSystem> particles;

    private void Awake()
    {
        particles = new Dictionary<MonsterParticleType, ParticleSystem>();
    }

    public ParticleSystem GetParticle(MonsterParticleType type)
    {
        if (particles.TryGetValue(type, out particle))
        {
            particle.transform.position = gameObject.transform.position + data.GetParticleOffset(type);
            return particle;
        }

        var particleObj = Instantiate(data.GetParticleSystem(type), transform);
        particles.Add(type, particleObj);
        if (particles.TryGetValue(type, out particle))
        {
            particle.transform.position = gameObject.transform.position + data.GetParticleOffset(type);
            return particle;
        }

        return null;
    }

    public bool PlayParticle(MonsterParticleType type)
    {
        particle = GetParticle(type);
        if (particle == null)
        {
            return false;
        }

        particle.Play();
        return true;
    }

    public void StopParticle()
    {
        if (particle != null)
        {
            particle.Stop();
        }
    }
}