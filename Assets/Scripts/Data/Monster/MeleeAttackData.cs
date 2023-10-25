using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestSkeletonMonsterData", menuName = "GameData List/Monsters/MeleeAttackData", order = int.MaxValue)]
public class MeleeAttackData : ScriptableObject
{
        public float attackableDistance;
        public float attackInterval;
        public float attackDuration;
        public float animationHold;
}
