using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
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
        Vector3 direction = playerPos.Value - context.transform.position;
        float dot = Vector3.Dot(direction.normalized, context.transform.forward.normalized);
        if (dot > 0)
            return State.Success;
        else return State.Failure;
    }
}
