using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class BooleanToFalse : ActionNode
{
    public NodeProperty<bool> boolean;
    protected override void OnStart() {
        boolean.Value = false;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
