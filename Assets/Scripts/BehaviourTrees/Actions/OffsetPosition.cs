using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class OffsetPosition : ActionNode
{
    public NodeProperty<Vector3> offset;

    protected override void OnStart()
    {
        context.transform.position += offset.Value;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}