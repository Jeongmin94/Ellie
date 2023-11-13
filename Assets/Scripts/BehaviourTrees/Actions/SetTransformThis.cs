using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class SetTransformThis : ActionNode
{
    public NodeProperty<Transform> targetTransform;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        targetTransform.Value = context.transform;

        return State.Success;
    }
}
