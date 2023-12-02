namespace Assets.Scripts.Monsters.Utility
{

    public static class Enums
    {
        //Monster
        //Monster Type
        public enum MonsterElement
        {
            Normal, Fire, Water, Grass, Earth, Light, Dark
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

        //Monster State
        public enum MonsterState
        {
            Wait, Patrol, Chase, Attack, Return, End
        }


        //Monster Skill
    }

}