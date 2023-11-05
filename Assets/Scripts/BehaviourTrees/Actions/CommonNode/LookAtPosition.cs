using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class LookAtPosition : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<Transform> targetTransform;

    private Vector3 targetPos;

    protected override void OnStart() {
        targetPos = targetTransform.Value != null ? targetTransform.Value.position : targetPosition.Value;
        targetPos.y = context.transform.position.y;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.transform.LookAt(targetPos);

        return State.Success;
    }
}
