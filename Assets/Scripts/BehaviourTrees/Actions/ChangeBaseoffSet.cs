using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ChangeBaseoffSet : ActionNode
{
    public NodeProperty<float> offsetValue;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.agent.baseOffset = offsetValue.Value;
        return State.Success;
    }
}
