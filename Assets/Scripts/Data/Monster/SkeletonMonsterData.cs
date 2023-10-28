using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Monsters.AbstractClass;

namespace Assets.Scripts.Data
{

    [CreateAssetMenu(fileName = "TestSkeletonMonster",menuName = "GameData List/Monsters/SkeletonMonsterData", order =int.MaxValue)]
    public class SkeletonMonsterData : ScriptableObject
    {
        [SerializeField] public int monsterID;
        [SerializeField] public string monsterName;

        [SerializeField] public float HP;
        [SerializeField] public float movementSpeed;
        [SerializeField] public float rotationSpeed;
        [SerializeField] public float detectPlayerDistance;
        [SerializeField] public float chasePlayerDistance;
        [SerializeField] public float overtravelDistance;
        [SerializeField] public float stopDistance;
        [SerializeField] public Vector3 spawnPosition;

        [SerializeField] public Enums.MonsterKind kind;
        [SerializeField] public Enums.MovementType type;
        [SerializeField] public Enums.AttackTurnType turnType;

        [SerializeField] public Dictionary<string, AbstractAttack> skills;
    }
}