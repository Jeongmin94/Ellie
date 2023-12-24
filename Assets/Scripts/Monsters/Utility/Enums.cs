namespace Monsters.Utility
{
    public static class Enums
    {
        public enum AttackSkill
        {
            RunToPlayer,
            Flee,
            BoxCollider,
            SphereCollider,
            ProjectileAttack,
            WeaponAttack,
            AOEAttack,
            FanshapeAttack,
            End
        }

        public enum AttackTurnType
        {
            Offensive,
            Deffensive,
            End
        }

        //Monster
        //Monster Type
        public enum MonsterElement
        {
            Normal,
            Fire,
            Water,
            Grass,
            Earth,
            Light,
            Dark
        }

        //Monster State
        public enum MonsterState
        {
            Wait,
            Patrol,
            Chase,
            Attack,
            Return,
            End
        }

        public enum MovementType
        {
            Ground,
            Flying,
            GroundFlying,
            End
        }


        //Monster Skill
    }
}