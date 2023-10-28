using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxColliderAttack", menuName = "GameData List/Monsters/BoxColliderAttack", order = int.MaxValue)]
public class BoxColliderAttackData : ScriptableObject   
{
    public float attackValue;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;

    public Vector3 size;
    public Vector3 offset;

    public float animationHold;
    
}
