using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class EnemyData : ScriptableObject
{ 
    [Tooltip("Monster Index")]
    public int _id;
    public string _name;

    public int monsterDropTable;

    [Tooltip("Monster Info")]
    public int monsterHP;
    public float monsterMovement;

    public float monsterWeakRatio;

    public float monsterAttackRange;
    public float monsterAttackInterval;
}
