using System.Collections;
using System.Collections.Generic;
using Channels.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxColliderAttack", menuName = "GameData List/Monsters/BoxColliderAttack", order = int.MaxValue)]
public class FanShapeAttackData : ScriptableObject
{
    public string attackName;
    public CombatType combatType;
    public float attackValue;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;

    public Transform target;
    public float angleRange;
    public float radius;

    public float animationHold;
}
