using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class OvertravelBool : ActionNode
{
    public NodeProperty<Vector3> returnPosition;
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
        //if(Vector3.Distance(returnPosition.Value, context.transform.position)
        //    >context.controller.monsterData.overtravelDistance)
        if (distance > overtravelDist * overtravelDist)
        {
            return State.Success;
        }
        return State.Failure;
    }
}
