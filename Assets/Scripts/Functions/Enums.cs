using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
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
        BoxCollider, SphereCollider, ProjectileAttack, WeaponAttack, End
    }

    //Monster Skill
}
