using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class MoveDirection : ActionNode
{
    public NodeProperty<float> moveSpeed;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        context.transform.Translate(Vector3.forward * moveSpeed.Value * Time.deltaTime);

        return State.Success;
    }
}