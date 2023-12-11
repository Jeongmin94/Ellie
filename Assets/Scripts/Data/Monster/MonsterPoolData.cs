using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterAttackData", menuName = "GameData List/Monsters/AttackData", order = int.MaxValue)]
public class MonsterPoolData : ScriptableObject
{
    public Transform spawnPosition;
    public List<Transform> patrolPoints;
}
