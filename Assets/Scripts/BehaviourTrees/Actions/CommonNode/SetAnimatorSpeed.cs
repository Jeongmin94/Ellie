using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetAnimatorSpeed : ActionNode
{
    public NodeProperty<float> animationSpeed;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        context.animator.speed = animationSpeed.Value;
        return State.Success;
    }
}
