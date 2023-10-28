using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckTargetDistance : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<float> checkDistanceValue;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(Vector3.Distance(context.transform.position, targetPosition.Value) <= checkDistanceValue.Value)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
