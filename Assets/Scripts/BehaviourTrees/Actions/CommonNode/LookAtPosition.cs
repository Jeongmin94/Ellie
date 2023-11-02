using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class LookAtPosition : ActionNode
{
    public NodeProperty<Vector3> targetPosition;
    public NodeProperty<Transform> targetTransform;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(targetTransform.Value == null)
        {
            context.transform.LookAt(targetPosition.Value);
        }
        else
        {
            context.transform.LookAt(targetTransform.Value.position);
        }

        return State.Success;
    }
}
