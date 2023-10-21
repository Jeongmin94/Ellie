using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using Assets.Scripts.Monsters.AbstractClass;

[System.Serializable]
public class MeleeAttack : ActionNode
{
    public NodeProperty<GameObject> skillObject;
    public NodeProperty<float> skillActivateDelay;

    private AbstractAttack skill;
    private float accumulatedTime;

    protected override void OnStart() {
        skill = skillObject.Value.GetComponent<AbstractAttack>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.animator.SetTrigger("SkeletonMeleeAttack");

        while(accumulatedTime<skillActivateDelay.Value)
        {
            accumulatedTime += Time.deltaTime;
        }

        skill.ActivateAttack();
        return State.Success;
    }
}
