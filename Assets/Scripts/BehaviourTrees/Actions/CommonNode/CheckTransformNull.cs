using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class CheckTransformNull : ActionNode
{
    public NodeProperty<Transform> checkTransform;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return checkTransform.Value != null ? State.Success : State.Failure;
    }
}