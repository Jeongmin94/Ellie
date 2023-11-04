using System.Collections;
using System.Collections.Generic;
using Channels.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "GameData List/Monsters/MeleeAttackData", order = int.MaxValue)]
public class MeleeAttackData : ScriptableObject
{
    public CombatType combatType;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;
    public float animationHold;
}
