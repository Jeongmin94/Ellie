using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunToPlayer", menuName = "GameData List/Monsters/ProjectileAttack", order = int.MaxValue)]
public class ProjectileAttackData : ScriptableObject
{
    public float attackValue;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;

    public Vector3 offset;

    public GameObject projectilePrefab;

    public float animationHold;
}
