using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunToPlayer", menuName = "GameData List/Monsters/WeaponAttack", order = int.MaxValue)]
public class WeaponAttackData : ScriptableObject
{
    public float attackValue;
    public float attackableDistance;
    public float attackInterval;
    public float attackDuration;
      
    public GameObject weapon;

    public float animationHold;
}
