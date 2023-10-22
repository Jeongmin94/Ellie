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
        accumulatedTime = 0.0f;
        context.animator.SetTrigger("SkeletonAttack");
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {

        while (accumulatedTime <= skillActivateDelay.Value)
        {
            accumulatedTime += Time.deltaTime;
            return State.Running;
        }

        skill.ActivateAttack();
        return State.Success;
    }
}
