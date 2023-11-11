using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Monsters.Utility;
using Assets.Scripts.Monsters.AbstractClass;

//[SerializeField]
//public class SkeletonMonsterData
//{

//}

namespace Assets.Scripts.Data
{

    [CreateAssetMenu(fileName = "TestSkeletonMonster",menuName = "GameData List/Monsters/SkeletonMonsterData", order =int.MaxValue)]
    public class SkeletonMonsterData : ScriptableObject
    {
        public int monsterID;
        public string monsterName;

        public float maxHP;
        public float movementSpeed;
        public float rotationSpeed;
        public float detectPlayerDistance;
        public float chasePlayerDistance;
        public float overtravelDistance;
        public float stopDistance;

        public Enums.MonsterKind kind;
        public Enums.MovementType type;
        public Enums.AttackTurnType turnType;

        public Dictionary<string, AbstractAttack> skills;
    }
}