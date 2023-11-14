using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CheckTargetDistance : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<Transform> targetTransform;
    public NodeProperty<float> checkDistanceValue;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
        float checkDistanceSqr = checkDistanceValue.Value * checkDistanceValue.Value;

        // Vector3으로 비교
        if (targetTransform.Value == null)
        {
            if ((context.transform.position - targetPosition.Value).sqrMagnitude <= checkDistanceSqr)
            {
                return State.Success;
            }
        }
        // Transform으로 비교
        else
        {
            if ((context.transform.position - targetTransform.Value.position).sqrMagnitude <= checkDistanceSqr)
            {
                return State.Success;
            }
        }
        return State.Failure;
    }

}
