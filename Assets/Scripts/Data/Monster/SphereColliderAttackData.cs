using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SphereColliderAttack", menuName = "GameData List/Monsters/SphereColliderAttack", order = int.MaxValue)]
public class SphereColliderAttackData : ScriptableObject
{
    public float attackValue;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;

    public float radius;
    public Vector3 offset;

    public float animationHold;
}
