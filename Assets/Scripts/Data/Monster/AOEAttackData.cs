using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AOEAttack", menuName = "GameData List/Monsters/AOEAttackData", order = int.MaxValue)]
public class AOEAttackData : ScriptableObject
{
    public float attackValue;
    public float durationTime;
    public float attackInterval;
    public float attackableDistance;
    public float damageInterval;

    public GameObject prefabObject;
}
