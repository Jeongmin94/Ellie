using System.Collections;
using System.Collections.Generic;
using Channels.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAttack", menuName = "GameData List/Monsters/WeaponAttack", order = int.MaxValue)]
public class WeaponAttackData : ScriptableObject
{
    public CombatType combatType;
    public float attackValue;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;
      
    public GameObject weapon;

    public float animationHold;
}
