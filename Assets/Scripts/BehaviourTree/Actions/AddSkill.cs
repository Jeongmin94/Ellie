using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

using Assets.Scripts.Monsters.AbstractClass;
using Assets.Scripts.Monsters.Utility;

[System.Serializable]
public class AddSkill : ActionNode
{
    public NodeProperty<string> attackName;
    public NodeProperty<Enums.AttackSkill> attackType;


    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.controller.AddSkills(attackName.Value, attackType.Value);
        return State.Success;
    }
}
