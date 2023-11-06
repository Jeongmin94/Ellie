using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

using Assets.Scripts.Monsters.Utility;

[System.Serializable]
public class AddProjectileAttack : ActionNode
{
    public NodeProperty<string> attackName;
    public NodeProperty<string> projectilePrefabName;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.controller.AddSkills(attackName.Value, Enums.AttackSkill.ProjectileAttack);
        context.controller.Attacks[attackName.Value].InitializeProjectile
            (context.controller.projectileAttackData);

        return State.Success;
    }
}
