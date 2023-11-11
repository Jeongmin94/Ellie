using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class OvertravelBool : ActionNode
{
    public NodeProperty<Vector3> returnPosition;
    public NodeProperty<bool> isReturning;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float distance = Vector3.SqrMagnitude(returnPosition.Value - context.transform.position);
        float overtravelDist = context.controller.monsterData.overtravelDistance;
        if (distance > overtravelDist * overtravelDist)
        {
            isReturning.Value = true;
            return State.Success;
        }
        isReturning.Value = false;
        return State.Failure;
    }
}
