using System;
using System.Collections;
using System.Collections.Generic;
using Channels.Combat;
using UnityEngine;
using static Assets.Scripts.Monsters.Utility.Enums;

[Serializable]
public class MeleeAttackData
{
    public int index;
    public string monsterName;
    public string attackName;

    public AttackSkill attackType;
    public int attackValue;
    public int attackableDistance;
    public int attackInterval;
    public int attackDuration;
    public Vector3 size;
    public Vector3 offset;
    public float animationHold;
    public int attackablueMinimumDistance;


    //table 추가
    public string weaponName;
    public GameObject projectilePrefab;

}

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "GameData List/Monsters/MeleeAttackData", order = int.MaxValue)]
public class MeleeAttackData : ScriptableObject
{
    public CombatType combatType;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;
    public float animationHold;
}
