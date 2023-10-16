using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MonsterInfo : ScriptableObject
{
    [Tooltip("Monster Index")]
    public string monsterName;
    public int monsterIndex;
    public int monsterDropTable;

    [Tooltip("Monster Feature")]
    public Enums.AttackTurnType monsterAggressive;
    public List<Enums.StatusEffect> monsterImmunes;
    public Enums.Element monsterElement;

    [Tooltip("Monster Info")]
    public int monsterHP;
    public float monsterMovement;

    public float monsterWeakRatio;

    public float monsterAttackRange;
    public float monsterAttackInterval;
}
