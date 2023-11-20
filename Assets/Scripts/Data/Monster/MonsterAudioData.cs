using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterAudio", menuName = "GameData List/Monsters/MonsterAudioData", order = int.MaxValue)]
public class MonsterAudioData : ScriptableObject
{
    public AudioClip idle;
    public AudioClip idleAttack1;
    public AudioClip idleAttack2;
    public AudioClip move1;
    public AudioClip move2;
    public AudioClip hit;
    public AudioClip dead;
    public AudioClip moveSkill;
    public AudioClip meleeAttack;
    public AudioClip meleeAttackHit;
    public AudioClip weaponAttack;
    public AudioClip weaponAttackPerform;
    public AudioClip weaponAttackHit;
    public AudioClip projectileAttack;
    public AudioClip projectileFire;
    public AudioClip projectileHit;
    public AudioClip bite;
    public AudioClip biteHit;

    public AudioClip GetAudioClip(MonsterAudioType type)
    {
        switch (type)
        {
            case MonsterAudioType.Idle:
                return idle;
            case MonsterAudioType.IdleAttack1:
                return idleAttack1;
            case MonsterAudioType.IdleAttack2:
                return idleAttack2;
            case MonsterAudioType.Move1:
                return move1;
            case MonsterAudioType.Move2:
                return move2;
            case MonsterAudioType.Hit:
                return hit;
            case MonsterAudioType.Dead:
                return dead;
            case MonsterAudioType.MoveSkill:
                return moveSkill;
            case MonsterAudioType.MeleeAttack:
                return meleeAttack;
            case MonsterAudioType.MeleeAttackHit:
                return meleeAttackHit;
            case MonsterAudioType.WeaponAttack:
                return weaponAttack;
            case MonsterAudioType.WeaponAttackPerform:
                return weaponAttackPerform;
            case MonsterAudioType.WeaponAttackHit:
                return weaponAttackHit;
            case MonsterAudioType.ProjectileAttack:
                return projectileAttack;
            case MonsterAudioType.ProjectileFire:
                return projectileFire;
            case MonsterAudioType.ProjectileHit:
                return projectileHit;
        }
        return null;
    }
}