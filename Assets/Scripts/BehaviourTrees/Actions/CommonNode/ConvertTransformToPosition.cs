using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class ConvertTransformToPosition : ActionNode
{
    public NodeProperty<Transform> startTransform;
    public NodeProperty<Vector3> resultPosition;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        resultPosition.Value = startTransform.Value.position;

        return State.Success;
    }
}