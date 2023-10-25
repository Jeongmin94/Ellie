using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

using Assets.Scripts.Monsters.Utility;

[System.Serializable]
public class AddBoxColliderAttack : ActionNode
{
    public NodeProperty<string> attackName;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.controller.AddSkills(attackName.Value, Enums.AttackSkill.BoxCollider);
        context.controller.Attacks[attackName.Value].InitializeBoxCollider
            (context.controller.meleeAttackData);

        return State.Success;
    }
}
