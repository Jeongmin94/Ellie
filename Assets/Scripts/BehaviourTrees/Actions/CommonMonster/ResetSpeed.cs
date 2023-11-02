using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ResetSpeed : ActionNode
{
    public NodeProperty<float> defaultSpeed;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.agent.speed = defaultSpeed.Value;
        return State.Success;
    }
}
