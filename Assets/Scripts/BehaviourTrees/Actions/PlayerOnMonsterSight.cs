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
        Vector3 directionToPlayer = playerPos.Value - context.transform.position;
        float angle = Vector3.Angle(context.transform.forward, directionToPlayer);

        if(angle<70.0f*0.5f)
        {
            return State.Success;
        }

        return State.Failure;
    }
}
