using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class OffsetPosition : ActionNode
{
    public NodeProperty<Vector3> offset;
    protected override void OnStart() {
        context.transform.position += offset.Value;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return State.Success;
    }
}
