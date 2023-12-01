using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Channels.Combat;

[CreateAssetMenu(fileName = "AOEAttack", menuName = "GameData List/Monsters/AOEAttackData", order = int.MaxValue)]
public class AOEAttackData : ScriptableObject
{
    public CombatType combatType;
    public float attackValue;
    public float durationTime;
    public float attackInterval;
    public float attackableDistance;
    public float damageInterval;

    public GameObject prefabObject;
}
