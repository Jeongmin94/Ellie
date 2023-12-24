using UnityEngine;

namespace Monsters.Utility
{
    public class Structures : MonoBehaviour
    {
        public struct MonsterStat
        {
            public float HP;
            public float movementSpeed;
            public float rotationSpeed;
            public float detectPlayerDistance;
            public float overtravelDistance;
        }

        public struct MonsterType
        {
            public Enums.MovementType type;
            public Enums.AttackTurnType turnType;
        }
    }
}