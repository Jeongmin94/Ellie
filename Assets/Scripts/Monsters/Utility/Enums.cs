using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monsters.Utility
{

    public static class Enums
    {
        //Monster
        //Monster Type
        public enum MonsterKind
        {
            Human, Skeleton, Ghost, Insect, Beast, FlyingBeast, Golem, End
        }
        public enum MovementType
        {
            Ground, Flying, GroundFlying, End
        }
        public enum AttackTurnType
        {
            Offensive, Deffensive, End
        }
        public enum AttackSkill
        {
            BoxCollider, SphereCollider, ProjectileAttack, WeaponAttack, AOEAttack, End
        }

        //Monster State
        public enum MonsterState
        {
            Wait, Patrol, Chase, Attack, Return, End
        }


        //Monster Skill
    }

}