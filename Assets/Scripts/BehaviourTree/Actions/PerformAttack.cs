using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Monsters.AbstractClass;

[System.Serializable]
public class PerformAttack : ActionNode
{
    public NodeProperty<string> skillName;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        if(context.controller.Attacks.TryGetValue(skillName.Value, out AbstractAttack atk))
        {
            atk.ActivateAttack();
            Debug.Log(skillName.Value+ "  Attack");
            return State.Success;
        }

        Debug.LogError(context.gameObject + "SKILL UNFOUND");
        return State.Failure;
    }
}
