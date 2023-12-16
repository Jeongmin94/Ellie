using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterParticleController;

[CreateAssetMenu(fileName = "MonsterParticle", menuName = "GameData List/Monsters/MonsterParticleData", order = int.MaxValue)]
public class MonsterParticleData : ScriptableObject
{
    public ParticleSystem meleeAttack;
    public Vector3 meleeAttackOffset;
    public ParticleSystem meleeHit;
    public Vector3 meleeHitOffset;
    public ParticleSystem weaponAttack;
    public Vector3 weaponAttackOffset;
    public ParticleSystem weaponHit;
    public Vector3 weaponHitOffset;
    public ParticleSystem projectileCast;
    public Vector3 projectileCastOffset;
    public ParticleSystem projectileHit;
    public Vector3 projectileHitOffset;
    public ParticleSystem hitParticle;
    public Vector3 hitParticleOffset;
    public ParticleSystem headShotParticle;
    public Vector3 headShotParticleOffset; 

    public ParticleSystem GetParticleSystem(MonsterParticleType type)
    {
        switch (type)
        {
            case MonsterParticleType.MeleeAttack:
                return meleeAttack;
            case MonsterParticleType.MeleeHit:
                return meleeHit;
            case MonsterParticleType.WeaponAttack:
                return weaponAttack;
            case MonsterParticleType.WeaponHit:
                return weaponHit;
            case MonsterParticleType.ProjectileCast:
                return projectileCast;
            case MonsterParticleType.ProjectileHit:
                return projectileHit;
            case MonsterParticleType.Hit:
                return hitParticle;
            case MonsterParticleType.HeadShot:
                return headShotParticle;
        }

        return null;
    }

    public Vector3 GetParticleOffset(MonsterParticleType type)
    {
        switch (type)
        {
            case MonsterParticleType.MeleeAttack:
                return meleeAttackOffset;
            case MonsterParticleType.MeleeHit:
                return meleeHitOffset;
            case MonsterParticleType.WeaponAttack:
                return weaponAttackOffset;
            case MonsterParticleType.WeaponHit:
                return weaponHitOffset;
            case MonsterParticleType.ProjectileCast:
                return projectileCastOffset;
            case MonsterParticleType.ProjectileHit:
                return projectileHitOffset;
            case MonsterParticleType.Hit:
                return hitParticleOffset;
            case MonsterParticleType.HeadShot:
                return headShotParticleOffset;
        }

        return Vector3.zero;
    }
}