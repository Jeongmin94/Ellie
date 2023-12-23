using UnityEngine;
using static EffectController;

public class MonsterEffectData : ScriptableObject
{
    public ParticleSystem castFireball;
    public ParticleSystem meleeAttack;
    public ParticleSystem meleeAttackHit;
    public ParticleSystem weaponAttackSwing;
    public ParticleSystem weaponAttackHit;

    public ParticleSystem GetParticle(ParticleType type)
    {
        switch (type)
        {
            case ParticleType.CastFireball:
                return castFireball;
            case ParticleType.MeleeAttack:
                return meleeAttack;
            case ParticleType.MeleeAttackHit:
                return meleeAttackHit;
            case ParticleType.WeaponAttackSwing:
                return weaponAttackSwing;
            case ParticleType.WeaponAttackhit:
                return weaponAttackHit;
        }

        return null;
    }
}