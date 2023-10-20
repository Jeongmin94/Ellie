using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Monsters.Utility;

namespace Assets.Scripts.Data
{

    public class SkeletonMeleeMonsterData : ScriptableObject
    {
        public float HP;
        public float movementSpeed;
        public float rotationSpeed;
        public float detectPlayerDistance;
        public float chasePlayerDistance;
        public float overtravelDistance;

        public Vector3 spawnPosition;
    
        public Enums.MonsterKind kind;
        public Enums.MovementType type;
        public Enums.AttackTurnType turnType;
    }
}