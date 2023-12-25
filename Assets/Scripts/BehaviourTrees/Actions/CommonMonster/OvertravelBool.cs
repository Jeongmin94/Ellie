using System;
using TheKiwiCoder;
using UnityEngine;

[Serializable]
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
        var distance = Vector3.SqrMagnitude(returnPosition.Value - context.transform.position);
        var overtravelDist = context.controller.monsterData.overtravelDistance;
        if (distance > overtravelDist * overtravelDist)
        {
            isReturning.Value = true;
            return State.Success;
        }

        isReturning.Value = false;
        return State.Failure;
    }
}