using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RunToPlayer",menuName = "GameData List/Monsters/RunToPlayer",order =int.MaxValue)]
public class RunToPlayerAttackData : ScriptableObject
{
    public float movementSpeed;
    public float attackInterval;
    public float attackDuration;
    public float stopDistance;
    public float readyHold;
    public float activateMinimumDistance;
}
