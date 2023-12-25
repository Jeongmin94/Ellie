using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
public class PlayerOnMonsterSight : ActionNode
{
    public NodeProperty<Vector3> playerPos;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var direction = playerPos.Value - context.transform.position;
        var dot = Vector3.Dot(direction.normalized, context.transform.forward.normalized);
        if (dot > 0)
        {
            return State.Success;
        }

        return State.Failure;
    }
}